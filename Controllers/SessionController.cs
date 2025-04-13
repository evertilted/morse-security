using Microsoft.AspNetCore.Mvc;
using morse_auth.Services;

namespace morse_auth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SessionController : ControllerBase
    {

        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetSessionToken")]
        public IActionResult GetToken()
        {
            return Ok(new TokenService().Get());
        }
    }
}
