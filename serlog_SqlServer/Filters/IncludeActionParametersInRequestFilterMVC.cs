using System;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace serlog_SqlServer.Filters
{
    /// <summary>
    /// Asp.Net MVC
    /// </summary>
    public class IncludeActionParametersInRequestFilterMvc : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            // HttpContext.Current.Items.Add("ActionParameters", actionContext.ActionParameters);
            var actionParametersLazy =
                new Lazy<string>(() => JsonConvert.SerializeObject(actionContext.ActionParameters));

            actionContext.RequestContext.RouteData.Values.Add("ActionParameters", actionParametersLazy);
        }
    }
}