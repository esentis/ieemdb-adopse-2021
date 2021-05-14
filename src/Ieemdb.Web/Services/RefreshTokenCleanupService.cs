namespace Esentis.Ieemdb.Web.Services
{
  using System;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Web.Options;

  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  public class RefreshTokenCleanupService : BackgroundService
  {
    private readonly Timer timer;
    private readonly IServiceScopeFactory factory;
    private readonly ILogger<RefreshTokenCleanupService> logger;
    private readonly JwtOptions jwtOptions;
    private readonly ServiceDurations serviceDurations;
    private CancellationTokenSource? cancellationTokenSource;

    public RefreshTokenCleanupService(
      IServiceScopeFactory scopeFactory,
      IOptions<JwtOptions> jwtOptions,
      IOptions<ServiceDurations> durations,
      ILogger<RefreshTokenCleanupService> logger)
    {
      factory = scopeFactory;
      this.logger = logger;
      this.jwtOptions = jwtOptions.Value;
      serviceDurations = durations.Value;
      timer = new Timer(Trigger, null,
        TimeSpan.Zero,
        TimeSpan.FromMinutes(this.serviceDurations.CleanupTokensInMinutes));
    }

    public void Dispose()
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
          var expired = DateTimeOffset.Now.AddDays(-jwtOptions.RefreshTokenDurationInDays);
          var tokens = context.Devices.Where(x => x.UpdatedAt < expired).ToList();
          context.Devices.RemoveRange(tokens);
          await context.SaveChangesAsync();
          logger.LogInformation("Removed  {Count} refresh tokens", tokens.Count);

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

  }
}
