using System.Web.Mvc;
using serlog_SqlServer.Filters;

namespace serlog_SqlServer
{
    /// <summary>
    /// Asp.Net MVC
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
             
            filters.Add(new ExceptionHandlerAttribute());

            filters.Add(new IncludeActionParametersInRequestFilterMvc());
        }
    }
}