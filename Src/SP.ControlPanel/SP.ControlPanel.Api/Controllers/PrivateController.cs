using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SP.ControlPanel.Api.Controllers
{
    [Authorize(Policy = "IsGlobalAdministrator")]
    [Route("[controller]")]
    [ApiController]
    public class PrivateController : Controller
    {
        public IActionResult Get()
        {
            return Ok("Hello World!");
        }
    }
}