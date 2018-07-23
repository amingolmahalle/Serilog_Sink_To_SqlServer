using System;
using System.Web.Mvc;

namespace serlog_SqlServer.Filters
{
    /// <summary>
    /// Asp.Net MVC
    /// </summary>
    public class ExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is InvalidOperationException) return;

            var actionParameters = filterContext.RouteData.Values.ContainsKey("ActionParameters")
                ? filterContext.RequestContext.RouteData.Values["ActionParameters"]
                : "";// filterContext.RouteData.Values.Add("test", "TESTING");

            Serilog.Log
                .ForContext("RequestedUrl", filterContext.HttpContext.Request.Url?.AbsoluteUri)// filterContext.RequestContext.HttpContext.Request.RawUrl
                .ForContext("ActionParameters", actionParameters)
                .Fatal(
                    filterContext.Exception,
                    filterContext.Exception.Message
                );
            filterContext.ExceptionHandled = true;
        }
    }
}