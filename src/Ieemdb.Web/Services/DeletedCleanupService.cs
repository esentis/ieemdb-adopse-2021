namespace Esentis.Ieemdb.Web.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;

  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;

  public class DeletedCleanupService : BackgroundService
  {
    private readonly Timer timer;
    private readonly ILogger<DeletedCleanupService> logger;
    private readonly IServiceScopeFactory factory;
    private CancellationTokenSource? cancellationTokenSource;

    public DeletedCleanupService(ILogger<DeletedCleanupService> logger, IServiceScopeFactory factory)
    {
      this.logger = logger;
      this.factory = factory;
      timer = new Timer(Trigger, null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
    }

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

        using var scope = factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IeemdbDbContext>();
        try
        {
          var timeToDelete = DateTimeOffset.Now.AddDays(-5);
          await DeleteMovies(context, timeToDelete, stoppingToken);
          await DeleteActors(context, timeToDelete, stoppingToken);
          await DeleteCountries(context, timeToDelete, stoppingToken);
          await DeleteWriter(context, timeToDelete, stoppingToken);
          await DeleteDirectors(context, timeToDelete, stoppingToken);
          await DeleteGenres(context, timeToDelete, stoppingToken);

          await context.SaveChangesAsync(stoppingToken);
        }
        catch (TaskCanceledException)
        {
          logger.LogInformation("Service {Service} is shutting down.", nameof(DeletedCleanupService));
        }
        catch (DbUpdateConcurrencyException)
        {
        }
        catch (DbUpdateException e)
        {
          logger.LogCritical(e, "Issues detected while saving to database: {Message}", e.Message);
        }
        catch (Exception e)
        {
          logger.LogError(e, "Unhandled exception: {Message}", e.Message);
        }
      }
    }

    private static async Task DeleteMovies(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedMovies =
        await ctx.Movies.Where(x => x.IsDeleted).Where(x => x.UpdatedAt < timeToDelete).ToListAsync(token);

      if (deletedMovies.Count != 0)
      {
        var movieRatings = await ctx.Ratings.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieActors = await ctx.MovieActors.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieDirectors = await ctx.MovieDirectors.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieWriters = await ctx.MovieWriters.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieGenres = await ctx.MovieGenres.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieCountries = await ctx.MovieCountries.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieScreenshots = await ctx.Screenshots.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);

        ctx.MovieCountries.RemoveRange(movieCountries);
        ctx.Ratings.RemoveRange(movieRatings);
        ctx.MovieActors.RemoveRange(movieActors);
        ctx.MovieDirectors.RemoveRange(movieDirectors);
        ctx.MovieWriters.RemoveRange(movieWriters);
        ctx.MovieGenres.RemoveRange(movieGenres);
        ctx.Screenshots.RemoveRange(movieScreenshots);

        ctx.Movies.RemoveRange(deletedMovies);
      }
    }

    private static async Task DeleteActors(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedActors =
        await ctx.Actors.Where(x => x.IsDeleted).Where(x => x.UpdatedAt < timeToDelete).ToListAsync(token);

      if (deletedActors.Count != 0)
      {
        var movieActors = await ctx.MovieActors.Where(x => deletedActors.Contains(x.Actor)).ToListAsync(token);

        ctx.MovieActors.RemoveRange(movieActors);
        ctx.Actors.RemoveRange(deletedActors);
      }
    }

    private static async Task DeleteCountries(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedCountries =
        await ctx.Countries.Where(x => x.IsDeleted).Where(x => x.UpdatedAt < timeToDelete).ToListAsync(token);

      if (deletedCountries.Count != 0)
      {
        var movieCountries =
          await ctx.MovieCountries.Where(x => deletedCountries.Contains(x.Country)).ToListAsync(token);

        ctx.MovieCountries.RemoveRange(movieCountries);
        ctx.Countries.RemoveRange(deletedCountries);
      }

      ctx.Countries.RemoveRange(deletedCountries);
    }

    private static async Task DeleteDirectors(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedDirectors =
        await ctx.Directors.Where(x => x.IsDeleted).Where(x => x.UpdatedAt < timeToDelete).ToListAsync(token);

      if (deletedDirectors.Count != 0)
      {
        var movieDirectors =
          await ctx.MovieDirectors.Where(x => deletedDirectors.Contains(x.Director)).ToListAsync(token);
        ctx.MovieDirectors.RemoveRange(movieDirectors);
        ctx.Directors.RemoveRange(deletedDirectors);
      }
    }

    private static async Task DeleteWriter(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedWriters =
        await ctx.Writers.Where(x => x.IsDeleted).Where(x => x.UpdatedAt < timeToDelete).ToListAsync(token);

      if (deletedWriters.Count != 0)
      {
        var movieWriters =
          await ctx.MovieWriters.Where(x => deletedWriters.Contains(x.Writer)).ToListAsync(token);
        ctx.MovieWriters.RemoveRange(movieWriters);
        ctx.Writers.RemoveRange(deletedWriters);
      }
    }

    private static async Task DeleteGenres(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedGenres =
        await ctx.Genres.Where(x => x.IsDeleted).Where(x => x.UpdatedAt < timeToDelete).ToListAsync(token);

      if (deletedGenres.Count != 0)
      {
        var movieGenres =
          await ctx.MovieGenres.Where(x => deletedGenres.Contains(x.Genre)).ToListAsync(token);
        ctx.MovieGenres.RemoveRange(movieGenres);
        ctx.Genres.RemoveRange(deletedGenres);
      }
    }

    private void Trigger(object? state) => cancellationTokenSource?.Cancel();
  }
}
