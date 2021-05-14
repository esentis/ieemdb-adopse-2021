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

          var allCountries = await context.Countries.ToListAsync(stoppingToken);

          List<DetailedMovie> moviesForSave = new();

          List<Person> peopleToBeSaved = new();

          List<MovieCast> castToBeSaved = new();

          var genres = await context.Genres.ToListAsync(stoppingToken);
          List<MovieGenre> movieGenres = new();

          foreach (var m in movies.results.Where(x => existingMovies.All(y => y.TmdbId != x.id)))
          {
            try
            {
              var detailedMovie = await tmdbApi.GetMovieDetails(m.id);
              var movieCast = await tmdbApi.GetMovieCredits(m.id);
              moviesForSave.Add(detailedMovie);
              castToBeSaved.Add(movieCast);
            }
            catch (ApiException e)
            {
              logger.LogError(e, "Error pasting data with Error: {Message} ", e.Message);
            }
          }

          var castIds = castToBeSaved.SelectMany(x => x.cast).Select(x => x.id).ToArray();
          var existingPeople =
            await context.People.Where(ac => castIds.Contains(ac.TmdbId)).ToListAsync(stoppingToken);

          foreach (var cast in castToBeSaved.Where(x =>
              existingPeople.All(y => x.cast.Any(all => all.id != y.TmdbId)))
            .SelectMany(x => x.cast))
          {
            try
            {
              var personForSave = await tmdbApi.GetPerson(cast.id);
              peopleToBeSaved.Add(new Person
              {
                Bio = personForSave.biography.Length > 700
                  ? personForSave.biography[..700]
                  : personForSave.biography,
                BirthDay = DateTime.TryParse(personForSave.birthday, out var birth)
                  ? birth
                  : null,
                DeathDay = DateTime.TryParse(personForSave.deathday, out var death)
                  ? death
                  : null,
                FullName = personForSave.name,
                Image = $"https://image.tmdb.org/t/p/w600_and_h900_bestv2{personForSave.profile_path}",
                TmdbId = personForSave.id,
                KnownFor = personForSave.known_for_department.Equals("Acting")
                  ? DepartmentEnums.Acting
#pragma warning disable S3358 // Ternary operators should not be nested
                  : personForSave.known_for_department.Equals("Directing")
                    ? DepartmentEnums.Directing
                    : personForSave.known_for_department.Equals("Writing")
                      ? DepartmentEnums.Writing
                      : personForSave.known_for_department.Equals("Production")
                        ? DepartmentEnums.Production
                        : personForSave.known_for_department.Equals("Editing")
                          ? DepartmentEnums.Editing
                          : personForSave.known_for_department.Equals("Art")
                            ? DepartmentEnums.Art
                            : personForSave.known_for_department.Equals("Sound")
                              ? DepartmentEnums.Sound
                              : DepartmentEnums.None,
#pragma warning restore S3358 // Ternary operators should not be nested
              });
            }
            catch (ApiException e)
            {
              logger.LogError(e, "Error pasting data for actor {Id} ", cast.id);
             
            }
          }

          foreach (var detailedMovie in moviesForSave)
          {
            var movieForSave = new Movie
            {
              Duration = detailedMovie.runtime == null ? TimeSpan.FromMinutes(0) : TimeSpan.FromMinutes(Convert.ToDouble(detailedMovie.runtime)),
              Plot = detailedMovie.overview,
              TmdbId = detailedMovie.id,
              ReleaseDate = DateTimeOffset.TryParse(detailedMovie.release_date, out var releaseDate) ? releaseDate : null,
              Title = detailedMovie.title,
              PosterUrl = $"https://image.tmdb.org/t/p/w600_and_h900_bestv2{detailedMovie.poster_path}",
            };

            var movieVideos = await tmdbApi.GetMovieVideos(detailedMovie.id);

            var movieImages = await tmdbApi.GetMovieImages(detailedMovie.id);

            var videos = movieVideos.results.Select(video => new Video
              {
                Movie = movieForSave,
                TmdbId = video.id,
                Key = video.key,
                Site = video.site,
                Type = video.type.Equals("Trailer")
                  ? VideoTypeEnums.Trailer
#pragma warning disable S3358 // Ternary operators should not be nested
                  : video.type.Equals("Teaser")
                    ? VideoTypeEnums.Teaser
                    : video.type.Equals("Teaser")
                      ? VideoTypeEnums.Teaser
                      : video.type.Equals("Clip")
                        ? VideoTypeEnums.Clip
                        : video.type.Equals("Featurette")
                          ? VideoTypeEnums.Featurette
                          : video.type.Equals("Behind the Scenes")
                            ? VideoTypeEnums.BehindTheScenes
                            : video.type.Equals("Bloopers")
                              ? VideoTypeEnums.Bloopers
                              : VideoTypeEnums.None,
#pragma warning restore S3358 // Ternary operators should not be nested
              })
              .ToList();

            var images = movieImages.posters.Select(p => new Image
            {
              Movie = movieForSave,
              Url = $"https://image.tmdb.org/t/p/w600_and_h900_bestv2{p.file_path}",
            })
              .ToList();

            var movieCountriesIsos = detailedMovie.production_countries.Select(c => c.iso_3166_1).ToList();

            var movieCountriesForSave = allCountries.Where(country => movieCountriesIsos.Contains(country.Iso))
              .Select(x => new MovieCountry { Country = x, Movie = movieForSave })
              .ToList();

            movieGenres.AddRange(detailedMovie.genres.Select(x => genres.SingleOrDefault(y => y.TmdbId == x.id))
              .Where(x => x != null)
              .Select(x => new MovieGenre { Genre = x, Movie = movieForSave, }));

            var moviePeople = peopleToBeSaved.Where(x =>
                castToBeSaved.Any(movieCast =>
                  movieCast.id == movieForSave.TmdbId && movieCast.cast.Any(z => z.id == x.TmdbId)))
              .Select(x => new MoviePerson { Person = x, Movie = movieForSave, })
              .ToList();

            context.Images.AddRange(images);
            context.Videos.AddRange(videos);
            context.MovieCountries.AddRange(movieCountriesForSave);
            context.MoviePeople.AddRange(moviePeople);
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
