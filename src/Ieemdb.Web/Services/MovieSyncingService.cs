namespace Esentis.Ieemdb.Web.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Joins;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Models.Enums;
  using Esentis.Ieemdb.Web.Options;
  using Esentis.Ieemdb.Web.Providers;
  using Esentis.Ieemdb.Web.Providers.Models;

  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  using Refit;

  using Genre = Esentis.Ieemdb.Persistence.Models.Genre;

  public class MovieSyncingService : BackgroundService
  {
    private readonly Timer timer;
    private readonly ITheMovieDb tmdbApi;
    private readonly IServiceScopeFactory factory;
    private readonly IeemdbDbContext ctx;
    private readonly ServiceDurations serviceDurations;
    private readonly ILogger<MovieSyncingService> logger;
    private CancellationTokenSource? cancellationTokenSource;

    public MovieSyncingService(
      IServiceScopeFactory factory,
      ITheMovieDb tmdb,
      ILogger<MovieSyncingService> logger,
      IOptions<ServiceDurations> durations)
    {
      this.factory = factory;
      tmdbApi = tmdb;
      serviceDurations = durations.Value;
      this.logger = logger;
      timer = new Timer(
        Trigger,
        null,
        TimeSpan.Zero,
        TimeSpan.FromSeconds(serviceDurations.DatabaseSeedingInMinutes));
    }

    public override void Dispose()
    {
      timer.Dispose();
      base.Dispose();
    }

    public void Trigger(object? state) => cancellationTokenSource?.Cancel();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        cancellationTokenSource = new CancellationTokenSource();

        try
        {
          using var synced =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token, stoppingToken);
          await Task.Delay(Timeout.Infinite, synced.Token);
        }
        catch (TaskCanceledException)
        {
        }

        if (stoppingToken.IsCancellationRequested)
        {
          return;
        }

        try
        {
          using var scope = factory.CreateScope();
          var context = scope.ServiceProvider.GetRequiredService<IeemdbDbContext>();
          var progress = await context.ServiceBatchingProgresses.Where(x => x.Name == BackgroundServiceName.MovieSync)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(stoppingToken);
          if (progress == null || progress.LastProccessedPage >= progress.TotalPages - 1)
          {
            progress = new ServiceBatchingProgress { Name = BackgroundServiceName.MovieSync, LastProccessedPage = 1, };
            context.ServiceBatchingProgresses.Add(progress);
          }
          else
          {
            progress.LastProccessedPage++;
          }

          var movies = await tmdbApi.GetPopular(progress.LastProccessedPage);
          progress.TotalPages = progress.LastProccessedPage == 1
            ? movies.total_pages
            : progress.TotalPages;

          var movieTMDBids = movies.results.Select(x => x.id).ToArray();

          var existingMovies =
            await context.Movies.Where(mv => movieTMDBids.Contains(mv.TmdbId)).ToListAsync(stoppingToken);

          List<DetailedMovie> moviesForSave = new();
          List<Actor> actorsToBeSaved = new();
          List<MovieCast> castToBeSavedd = new();
          var genres = await context.Genres.ToListAsync(stoppingToken);
          List<MovieGenre> movieGenres = new();

          foreach (var m in movies.results.Where(x => existingMovies.All(y => y.TmdbId != x.id)))
          {
            var detailedMovie = await tmdbApi.GetMovieDetails(m.id);
            var movieCast = await tmdbApi.GetMovieCredits(m.id);
            moviesForSave.Add(detailedMovie);
            castToBeSavedd.Add(movieCast);
          }

          var actorIds = castToBeSavedd.SelectMany(x => x.cast).Select(x => x.id).ToArray();
          var existingActors =
            await context.Actors.Where(ac => actorIds.Contains(ac.TmdbId)).ToListAsync(stoppingToken);

          foreach (var actor in castToBeSavedd.Where(x =>
              existingActors.All(y => x.cast.Any(all => all.id != y.TmdbId)))
            .SelectMany(x => x.cast))
          {
            try
            {
              var actorForSave = await tmdbApi.GetPerson(actor.id);
              actorsToBeSaved.Add(new Actor
              {
                Bio = actorForSave.biography.Length > 700
                  ? actorForSave.biography[..700]
                  : actorForSave.biography,
                BirthDate = DateTime.TryParse(actorForSave.birthday, out var birth)
                  ? birth
                  : null,
                FullName = actorForSave.name,
                TmdbId = actorForSave.id,
              });
            }
            catch (ApiException e)
            {
              logger.LogError(e, "Error pasting data for actor {Id} ", actor.id);
              throw;
            }
          }

          foreach (var m in moviesForSave)
          {
            var ms = new Movie
            {
              Duration = TimeSpan.FromMinutes(m.runtime),
              Plot = m.overview,
              TmdbId = m.id,
              ReleaseDate = DateTimeOffset.Parse(m.release_date),
              Title = m.title,
              PosterUrl = $"https://image.tmdb.org/t/p/w600_and_h900_bestv2{m.poster_path}",
              TrailerUrl = "",
            };
            movieGenres.AddRange(m.genres.Select(x => genres.SingleOrDefault(y => y.TmdbId == x.id))
              .Where(x => x != null)
              .Select(x => new MovieGenre { Genre = x, Movie = ms, }));
            var ma = actorsToBeSaved.Where(x =>
                castToBeSavedd.Any(y => y.id == ms.TmdbId && y.cast.Any(z => z.id == x.TmdbId)))
              .Select(x => new MovieActor
              {
                Actor = x,
                Movie = ms,
                Character = castToBeSavedd.SingleOrDefault(x => x.id == ms.TmdbId)
                  ?.cast.SingleOrDefault(c => c.id == x.TmdbId)
                  ?.character,
              })
              .ToList();
            context.MovieActors.AddRange(ma);
          }

          context.MovieGenres.AddRange(movieGenres);

          await context.SaveChangesAsync(stoppingToken);
        }
        catch (Exception e)
        {
          logger.LogCritical(e, "Unhandled exception caught: {Message}", e.Message);
        }
      }
    }
  }
}
