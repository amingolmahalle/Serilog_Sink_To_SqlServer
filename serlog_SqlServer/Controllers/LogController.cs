using System.Web.Http;

namespace serlog_SqlServer.Controllers
{
    public class LogController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAll(int ii)
        {
            //throw new ArgumentOutOfRangeException(nameof(ii), 111111111, "Api Error");

            for (var i = 0; i < 10; i++)
            {
                Serilog.Log.Logger.Information("Just a sample log. {Name:l}", "Api");
            }
            return Ok("Sample log added to local SqlServer.");
        }
    }
}
