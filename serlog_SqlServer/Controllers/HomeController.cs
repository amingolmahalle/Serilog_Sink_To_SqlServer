using System.Web.Mvc;

namespace serlog_SqlServer.Controllers
{
    /// <summary>
    /// Asp .Net MVC
    /// </summary>
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult GetAll(int id)
        {
            // throw new ArgumentOutOfRangeException(nameof(ii), 111111111, "MVC Error");

            for (var i = 0; i < id; i++)
            {
                Serilog.Log.Logger.Information("Just a sample log. {Name:l}", "MVC");
            }

            return View();
        }
    }
}