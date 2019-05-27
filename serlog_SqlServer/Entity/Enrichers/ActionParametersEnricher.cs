using System;
using System.Web;
using Serilog.Core;
using Serilog.Events;

namespace serlog_SqlServer.Entity.Enrichers
{
    public class ActionParametersEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var actionParametersLazy = HttpContext.Current.Items.Contains("ActionParameters")
                ? HttpContext.Current.Items["ActionParameters"] as Lazy<string>
                : new Lazy<string>();

            logEvent.AddPropertyIfAbsent(new LogEventProperty("ActionParameters", new ScalarValue(actionParametersLazy?.Value)));
        }
    }
}