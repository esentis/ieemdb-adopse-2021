namespace Esentis.Ieemdb.Web.ConfigurationOptions
{
  using Microsoft.Extensions.Logging;

  public class CorrelationOptions
  {
    public const string DefaultHeader = "X-Correlation-ID";

    public string Header { get; set; } = DefaultHeader;

    public bool IncludeInResponses { get; set; } = true;

    public LogLevel MissingCorrelationLogLevel { get; set; }
      = LogLevel.Debug;
  }
}
