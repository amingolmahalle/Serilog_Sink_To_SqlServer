using System.Web.Http;


namespace serlog_SqlServer.Controllers
{
    public class LogController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAll(int ii)
        {
            //throw new ArgumentOutOfRangeException("paraaaaaaamname", 111111111, "errrrorrr");

            for (var i = 0; i < 10; i++)
            {
                Serilog.Log.Logger.Information("Just a sample log. {Name:l}", "Amin");
            }
            return Ok("Sample log added to local SqlServer.");
        }
    }
}
