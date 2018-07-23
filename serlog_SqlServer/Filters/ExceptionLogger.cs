using System.Web.Http.ExceptionHandling;

namespace serlog_SqlServer.Filters
{
    /// <summary>
    /// Asp.Net WebApi
    /// </summary>
    public class ExceptionLogger : System.Web.Http.ExceptionHandling.ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var actionParameters = context.Request.Properties.ContainsKey("ActionParameters")
                ? context.Request.Properties["ActionParameters"]
                : "";

            Serilog.Log
                .ForContext("RequestedUrl", context.Request.RequestUri.AbsoluteUri)
                .ForContext("ActionParameters", actionParameters)
                .Fatal(
                    context.Exception,
                    context.Exception.Message
                );
        }
    }
}
