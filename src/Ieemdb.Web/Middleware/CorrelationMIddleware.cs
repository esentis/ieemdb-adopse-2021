namespace Esentis.Ieemdb.Web.Middleware
{
  using System;
  using System.Globalization;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Web.ConfigurationOptions;

  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  using Serilog.Context;

  public class CorrelationMIddleware : IMiddleware
  {
    private readonly ILogger<CorrelationMIddleware> logger;
    private readonly CorrelationOptions options;

    private const string MissingIdentifier = "Received request without correlation id, generated {Correlation}";

    public CorrelationMIddleware(IOptions<CorrelationOptions> options, ILogger<CorrelationMIddleware> logger)
    {
      this.options = options?.Value
                     ?? throw new ArgumentNullException(nameof(options));
      this.logger = logger;
    }

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      if (next == null)
      {
        throw new ArgumentNullException(nameof(next));
      }

      if (context.Request.Headers.TryGetValue(options.Header, out var value))
      {
        context.TraceIdentifier = value;
      }
      else
      {
        context.TraceIdentifier = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
        logger.Log(options.MissingCorrelationLogLevel, MissingIdentifier, context.TraceIdentifier);
      }

      if (options.IncludeInResponses)
      {
        context.Response.Headers.Add(options.Header, new[] { context.TraceIdentifier });
      }

      using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
      {
        return next(context);
      }
    }
  }
}
