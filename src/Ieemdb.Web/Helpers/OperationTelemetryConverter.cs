namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Collections.Generic;

  using Microsoft.ApplicationInsights.Channel;

  using Serilog.Events;
  using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

  public class OperationTelemetryConverter : TraceTelemetryConverter
  {
    #region Overrides of TraceTelemetryConverter

    /// <inheritdoc />
    public override IEnumerable<ITelemetry> Convert(LogEvent logEvent, IFormatProvider formatProvider)
    {
      foreach (var telemetry in base.Convert(logEvent, formatProvider))
      {
        if (TryGetScalarProperty(logEvent, "OperationId", out var operationId))
        {
          telemetry.Context.Operation.Id = operationId.ToString();
        }

        if (TryGetScalarProperty(logEvent, "ParentId", out var parentId))
        {
          telemetry.Context.Operation.ParentId = parentId?.ToString();
        }

        yield return telemetry;
      }
    }

    private bool TryGetScalarProperty(LogEvent logEvent, string propertyName, out object value)
    {
      var hasScalarValue =
        logEvent.Properties.TryGetValue(propertyName, out var someValue) && someValue is ScalarValue;
      value = hasScalarValue
        ? ((ScalarValue)someValue)?.Value
        : default;
      return hasScalarValue;
    }

    #endregion
  }
}
