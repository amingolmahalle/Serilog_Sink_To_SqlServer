using System.Web;
using Serilog.Core;
using Serilog.Events;

namespace serlog_SqlServer
{
    public class ActionParametersEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var actionParameters = HttpContext.Current.Items.Contains("ActionParameters")
                ? HttpContext.Current.Items["ActionParameters"]
                : "";

            logEvent.AddPropertyIfAbsent(new LogEventProperty("ActionParameters", new ScalarValue(actionParameters)));
        }
    }
}