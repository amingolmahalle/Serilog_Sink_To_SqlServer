using System.Web.Mvc;

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
            actionContext.RequestContext.RouteData.Values.Add("ActionParameters", actionContext.ActionParameters);
        }
    }
}