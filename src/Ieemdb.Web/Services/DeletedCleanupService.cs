namespace Esentis.Ieemdb.Web.Services
{
  using System;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Web.Models.Enums;
  using Esentis.Ieemdb.Web.Options;

  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  public class DeletedCleanupService : BackgroundService
  {
    private readonly Timer timer;
    private readonly ILogger<DeletedCleanupService> logger;
    private readonly IServiceScopeFactory factory;
    private readonly ServiceDurations serviceDurations;
    private CancellationTokenSource? cancellationTokenSource;

    public DeletedCleanupService(
      ILogger<DeletedCleanupService> logger,
      IServiceScopeFactory factory,
      IOptions<ServiceDurations> durations)
    {
      this.logger = logger;
      this.factory = factory;
      serviceDurations = durations.Value;
      timer = new Timer(Trigger, null, TimeSpan.Zero,
        TimeSpan.FromMinutes(this.serviceDurations.DeleteServiceInMinutes));
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
          await DeleteVideos(context, timeToDelete, stoppingToken);
          await DeleteImages(context, timeToDelete, stoppingToken);

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

    private async Task DeleteMovies(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedMovies =
        await ctx.Movies.IgnoreQueryFilters()
          .Where(x => x.IsDeleted)
          .Where(x => x.UpdatedAt < timeToDelete)
          .ToListAsync(token);

      if (deletedMovies.Count != 0)
      {
        var movieActors = await ctx.MoviePeople
          .Where(x => deletedMovies.Contains(x.Movie) && x.Person.KnownFor == DepartmentEnums.Acting)
          .ToListAsync(token);
        var movieDirectors = await ctx.MoviePeople
          .Where(x => deletedMovies.Contains(x.Movie) && x.Person.KnownFor == DepartmentEnums.Directing)
          .ToListAsync(token);
        var movieWriters = await ctx.MoviePeople
          .Where(x => deletedMovies.Contains(x.Movie) && x.Person.KnownFor == DepartmentEnums.Writing)
          .ToListAsync(token);

        var movieRatings = await ctx.Ratings.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieGenres = await ctx.MovieGenres.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieCountries = await ctx.MovieCountries.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);
        var movieScreenshots = await ctx.Images.Where(x => deletedMovies.Contains(x.Movie)).ToListAsync(token);

        ctx.MovieCountries.RemoveRange(movieCountries);
        ctx.Ratings.RemoveRange(movieRatings);
        ctx.MoviePeople.RemoveRange(movieActors);
        ctx.MoviePeople.RemoveRange(movieDirectors);
        ctx.MoviePeople.RemoveRange(movieWriters);
        ctx.MovieGenres.RemoveRange(movieGenres);
        ctx.Images.RemoveRange(movieScreenshots);

        logger.LogInformation("Removed  {Count} movies", deletedMovies.Count);
        ctx.Movies.RemoveRange(deletedMovies);
      }
    }

    private async Task DeleteActors(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedActors =
        await ctx.People.IgnoreQueryFilters()
          .Where(p => p.KnownFor == DepartmentEnums.Acting)
          .Where(x => x.IsDeleted)
          .Where(x => x.UpdatedAt < timeToDelete)
          .ToListAsync(token);

      if (deletedActors.Count != 0)
      {
        var movieActors = await ctx.MoviePeople.Where(p => p.Person.KnownFor == DepartmentEnums.Acting)
          .Where(x => deletedActors.Contains(x.Person))
          .ToListAsync(token);

        ctx.MoviePeople.RemoveRange(movieActors);
        ctx.People.RemoveRange(deletedActors);
        logger.LogInformation("Removed  {Count} actors", deletedActors.Count);
      }
    }

    private async Task DeleteCountries(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedCountries =
        await ctx.Countries.IgnoreQueryFilters()
          .Where(x => x.IsDeleted)
          .Where(x => x.UpdatedAt < timeToDelete)
          .ToListAsync(token);

      if (deletedCountries.Count != 0)
      {
        var movieCountries =
          await ctx.MovieCountries.Where(x => deletedCountries.Contains(x.Country)).ToListAsync(token);

        ctx.MovieCountries.RemoveRange(movieCountries);
        ctx.Countries.RemoveRange(deletedCountries);
        logger.LogInformation("Removed  {Count} countries", deletedCountries.Count);
      }

      ctx.Countries.RemoveRange(deletedCountries);
    }

    private async Task DeleteDirectors(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedDirectors =
        await ctx.People.IgnoreQueryFilters()
          .Where(p => p.KnownFor == DepartmentEnums.Directing)
          .Where(x => x.IsDeleted)
          .Where(x => x.UpdatedAt < timeToDelete)
          .ToListAsync(token);

      if (deletedDirectors.Count != 0)
      {
        var movieDirectors =
          await ctx.MoviePeople.Where(x => deletedDirectors.Contains(x.Person)).ToListAsync(token);
        ctx.MoviePeople.RemoveRange(movieDirectors);
        ctx.People.RemoveRange(deletedDirectors);
        logger.LogInformation("Removed  {Count} directors", deletedDirectors.Count);
      }
    }

    private async Task DeleteWriter(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedWriters =
        await ctx.People.IgnoreQueryFilters()
          .Where(p => p.KnownFor == DepartmentEnums.Writing)
          .Where(x => x.IsDeleted)
          .Where(x => x.UpdatedAt < timeToDelete)
          .ToListAsync(token);

      if (deletedWriters.Count != 0)
      {
        var movieWriters =
          await ctx.MoviePeople.Where(x => deletedWriters.Contains(x.Person)).ToListAsync(token);
        ctx.MoviePeople.RemoveRange(movieWriters);
        ctx.People.RemoveRange(deletedWriters);
        logger.LogInformation("Removed  {Count} writers", deletedWriters.Count);
      }
    }

    private async Task DeleteGenres(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deletedGenres =
        await ctx.Genres.IgnoreQueryFilters()
          .Where(x => x.IsDeleted)
          .Where(x => x.UpdatedAt < timeToDelete)
          .ToListAsync(token);

      if (deletedGenres.Count != 0)
      {
        var movieGenres =
          await ctx.MovieGenres.Where(x => deletedGenres.Contains(x.Genre)).ToListAsync(token);
        ctx.MovieGenres.RemoveRange(movieGenres);
        ctx.Genres.RemoveRange(deletedGenres);
        logger.LogInformation("Removed  {Count} genres", deletedGenres.Count);
      }
    }

    private async Task DeleteVideos(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deleteVideos =
        await ctx.Videos.IgnoreQueryFilters()
          .Where(x => x.IsDeleted)
          .Where(x => x.UpdatedAt < timeToDelete)
          .ToListAsync(token);

      if (deleteVideos.Count != 0)
      {
        ctx.Videos.RemoveRange(deleteVideos);
        logger.LogInformation("Removed  {Count} genres", deleteVideos.Count);
      }
    }

    private async Task DeleteImages(IeemdbDbContext ctx, DateTimeOffset timeToDelete, CancellationToken token)
    {
      var deleteImages =
        await ctx.Images.IgnoreQueryFilters()
          .Where(x => x.IsDeleted)
          .Where(x => x.UpdatedAt < timeToDelete)
          .ToListAsync(token);

      if (deleteImages.Count != 0)
      {
        ctx.Images.RemoveRange(deleteImages);
        logger.LogInformation("Removed  {Count} genres", deleteImages.Count);
      }
    }
  }
}
