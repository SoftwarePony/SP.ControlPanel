using Microsoft.AspNetCore.Mvc;

namespace SP.ControlPanel.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PublicController : Controller
    {
        public IActionResult Get()
        {
            return Ok("Hello World!");
        }
    }
}