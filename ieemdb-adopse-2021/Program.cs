using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Kritikos.StructuredLogging.Templates;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.SystemConsole.Themes;

namespace ieemdb_adopse_2021
{
    public static class Program
    {

        private static readonly string ApplicationName =
#pragma warning disable CS8602 // Can't be nulled.
#pragma warning disable CS8601 // Possible null reference assignment.
            Assembly.GetAssembly(typeof(Program)).GetName().Name;

        private static readonly LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().CreateBasicLogger().CreateLogger();
            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Logger = new LoggerConfiguration()
                    .CreateActualLogger(
                        host.Services.GetRequiredService<IConfiguration>(),
                        host.Services.GetRequiredService<IHostEnvironment>())
                    .CreateLogger();

                await host.RunAsync();
            }
#pragma warning disable CA1031 // Unhandled exception, application terminated
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Log.Fatal(e, GenericLogTemplates.UnhandledException, e.Message);
            }
            finally
            {
                Log.Information("{Application} shutting down", ApplicationName);
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static LoggerConfiguration CreateBasicLogger(this LoggerConfiguration logger)
            => logger
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", ApplicationName)
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                    .WithDefaultDestructurers()
                    .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })
                    .WithRootName("Exception"))
                .WriteTo.Debug()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code);

        public static LoggerConfiguration CreateActualLogger(this LoggerConfiguration logger,
            IConfiguration configuration, IHostEnvironment environment) =>
            logger.CreateBasicLogger()
                .Enrich.WithProperty("Application", environment.ApplicationName)
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .WriteTo.Logger(log => log
                    .MinimumLevel.ControlledBy(LevelSwitch)
                    .WriteTo.File(
                        Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"{environment.ApplicationName}-.log"),
                        fileSizeLimitBytes: 31457280,
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: 10,
                        shared: true));
    }
}
