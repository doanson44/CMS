using Microsoft.AspNetCore.Mvc;

namespace CMS.Scheduler
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}