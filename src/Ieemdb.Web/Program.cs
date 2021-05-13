namespace Esentis.Ieemdb.Web
{
  using System;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Helpers.Extensions;

  using Kritikos.Configuration.Persistence.Extensions;

  using Microsoft.ApplicationInsights.Extensibility;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Logging.Abstractions;

  using Serilog;
  using Serilog.Core;
  using Serilog.Extensions.Logging;

  public static class Program
  {
    internal static readonly LoggingLevelSwitch LevelSwitch = new();
    private static Microsoft.Extensions.Logging.ILogger logger = new NullLogger<Startup>();

    public static async Task<int> Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
        .CreateStartupLogger()
        .CreateBootstrapLogger();

      using var loggerProvider = new SerilogLoggerProvider(Log.Logger);
      logger = loggerProvider.CreateLogger(nameof(Startup));

      try
      {
        var host = CreateHostBuilder(args).Build();
        logger = host.Services.GetRequiredService<ILogger<Startup>>();
        await host.MigrateAsync<IeemdbDbContext>();
        await host.Services.SeedDatabase();

        await host.RunAsync();
        return 0;
      }
      catch (Exception e)
      {
        logger.LogCritical(e, LogTemplates.BootstrappingError, e.Message);
      }

      return 1;
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .UseSerilog((_, services, configuration) =>
          configuration.ConfigureLogger(
            services.GetRequiredService<IConfiguration>(),
            services.GetRequiredService<IWebHostEnvironment>(),
            services.GetRequiredService<TelemetryConfiguration>()))
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
        });
  }
}
