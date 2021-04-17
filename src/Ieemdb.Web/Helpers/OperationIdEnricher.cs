namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Threading.Tasks;

  using Serilog.Core;
  using Serilog.Events;

  public class OperationIdEnricher : ILogEventEnricher
  {
    #region Implementation of ILogEventEnricher

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
      var activity = Activity.Current;

      if (activity == null)
      {
        return;
      }

      logEvent.AddPropertyIfAbsent(new LogEventProperty("OperationId", new ScalarValue(activity.Id)));
      logEvent.AddPropertyIfAbsent(new LogEventProperty("ParentId", new ScalarValue(activity.Parent?.Id)));
    }

    #endregion
  }
}