namespace Esentis.Ieemdb.Web.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Options;
  using Esentis.Ieemdb.Web.Providers;

  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  public class GenreMetadataService : BackgroundService
  {
    private readonly Timer timer;
    private readonly ITheMovieDb tmdbApi;
    private readonly IServiceScopeFactory factory;
    private readonly IeemdbDbContext ctx;
    private readonly ServiceDurations serviceDurations;
    private readonly ILogger<MoviesMetadataUpdateService> logger;
    private CancellationTokenSource? cancellationTokenSource;

    public GenreMetadataService(
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
        TimeSpan.FromSeconds(120));
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

          var genres = await tmdbApi.GetGenres();
          var genreIds = genres.genres.Select(x => x.id).ToArray();

          var existingGenres = await context.Genres.Where(x => genreIds.Contains(x.TmdbId)).ToListAsync(stoppingToken);

          foreach (var genre in genres.genres.Where(x => existingGenres.Any(y => y.TmdbId != x.id)))
          {
            var genreToSave = new Genre() { TmdbId = genre.id, Name = genre.name, };
            context.Genres.Add(genreToSave);
          }

          var count = await context.SaveChangesAsync();
          logger.LogInformation("Added {Count} genres", count);
        }
        catch (Exception e)
        {
          logger.LogCritical(e, "Unhandled exception caught: {Message}", e.Message);
        }
      }
    }
  }
}
