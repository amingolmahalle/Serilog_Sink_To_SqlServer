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
            Serilog.Log
                .ForContext("RequestedUrl", context.Request.RequestUri.AbsoluteUri)
                .Fatal(
                    context.Exception,
                    "An Unhandled Error Occured: {ExceptionMessage}",
                    context.Exception.Message
                );
        }
    }
}
