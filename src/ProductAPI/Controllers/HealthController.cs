using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Controllers
{
    [ApiController]
    public class HealthController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("/health/check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}