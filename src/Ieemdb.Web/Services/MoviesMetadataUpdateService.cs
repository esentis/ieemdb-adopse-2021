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
  using Esentis.Ieemdb.Web.Options;
  using Esentis.Ieemdb.Web.Providers;
  using Esentis.Ieemdb.Web.Providers.Models;

  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  using Refit;

  public class MoviesMetadataUpdateService : BackgroundService
  {
    private readonly Timer timer;
    private readonly ITheMovieDb tmdbApi;
    private readonly IServiceScopeFactory factory;
    private readonly IeemdbDbContext ctx;
    private readonly ServiceDurations serviceDurations;
    private readonly ILogger<MoviesMetadataUpdateService> logger;
    private CancellationTokenSource? cancellationTokenSource;

    public MoviesMetadataUpdateService(
      IServiceScopeFactory factory,
      ITheMovieDb tmdb,
      ILogger<MoviesMetadataUpdateService> logger,
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
        TimeSpan.FromSeconds(60));
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
          var movies = await tmdbApi.GetRecommended();

          var movieTMDBids = movies.results.Select(x => x.id).ToArray();

          var existingMovies =
            await context.Movies.Where(mv => movieTMDBids.Contains(mv.TmdbId)).ToListAsync(stoppingToken);

          List<DetailedMovie> moviesForSave = new();
          List<Actor> actorsToBeSaved = new();
          List<MovieCast> castToBeSavedd = new();

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
