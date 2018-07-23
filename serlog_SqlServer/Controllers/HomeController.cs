using System.Web.Mvc;

namespace serlog_SqlServer.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult GetAll(int ii)
        {
            // throw new ArgumentOutOfRangeException(nameof(ii), 111111111, "MVC Error");

            for (var i = 0; i < 10; i++)
            {
                Serilog.Log.Logger.Information("Just a sample log. {Name:l}", "MVC");
            }
            return View();
        }
    }
}