using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace serlog_SqlServer.Filters
{
    public class IncludeActionParametersInRequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            HttpContext.Current.Items.Add("ActionParameters", actionContext.ActionArguments);
        }
    }
}