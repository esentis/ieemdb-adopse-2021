namespace Esentis.Ieemdb.Web.Helpers.Extensions
{
  using System;

  using Esentis.Ieemdb.Web.ConfigurationOptions;
  using Esentis.Ieemdb.Web.Middleware;

  using Microsoft.AspNetCore.Builder;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Options;

  public static class MiddlewareExtensions
  {
    public static IApplicationBuilder UseCorrelation(this IApplicationBuilder app)
    {
      if (app == null)
      {
        throw new ArgumentNullException(nameof(app));
      }

      return app.UseMiddleware<CorrelationMIddleware>();
    }

    public static IServiceCollection AddCorrelation(this IServiceCollection services, CorrelationOptions options)
    {
      if (options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      services?.AddSingleton(Options.Create(options));
      return services?.AddSingleton<CorrelationMIddleware>()
             ?? throw new ArgumentNullException(nameof(services));
    }

    public static IServiceCollection AddCorrelation(this IServiceCollection services)
      => services.AddCorrelation(new CorrelationOptions());
  }
}
