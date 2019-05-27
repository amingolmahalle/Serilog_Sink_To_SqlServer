using System.Web.Http;

namespace serlog_SqlServer.Controllers
{
    /// <summary>
    /// Web Api 
    /// </summary>
    public class WebApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll(int id)
        {
            //throw new ArgumentOutOfRangeException(nameof(ii), 111111111, "Api Error");

            for (var i = 0; i < id; i++)
            {
                Serilog.Log.Logger.Information("Just a sample log. {Name:l}", "Api");
            }

            return Ok("Sample log added to local SqlServer.");
        }
    }
}
