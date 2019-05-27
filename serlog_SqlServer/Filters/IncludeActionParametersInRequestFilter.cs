using System;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;

namespace serlog_SqlServer.Filters
{
    /// <summary>
    /// Asp.Net WebApi
    /// </summary>
    public class IncludeActionParametersInRequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var actionParametersLazy =
                new Lazy<string>(() => JsonConvert.SerializeObject(actionContext.ActionArguments));

            HttpContext.Current.Items.Add("ActionParameters", actionParametersLazy);
        }
    }
}