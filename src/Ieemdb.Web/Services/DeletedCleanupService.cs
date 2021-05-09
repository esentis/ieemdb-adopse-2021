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
      timer = new Timer(Trigger, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
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

      ctx.Movies.RemoveRange(deletedMovies);
    }

    private void Trigger(object? state) => cancellationTokenSource?.Cancel();
  }
}
