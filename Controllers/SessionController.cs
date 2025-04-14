using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using morse_auth.DTO;
using morse_auth.Services;

namespace morse_auth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;

        private SessionEnctyptionService _sessionEncryptionService = new SessionEnctyptionService();

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetSessionEncryptionKey()
        {
            return Ok(_sessionEncryptionService.GetPublicEncryptionKey());
        }

        [HttpPost(Name = "GetSessionToken")]
        public IActionResult GetSessionToken([FromBody]LoginRequestDTO data)
        {
            if (string.IsNullOrEmpty(data.Login) || string.IsNullOrEmpty(data.Password))
            {
                return BadRequest("Please provide a valid login and password");
            }

            return Ok(_sessionEncryptionService.GetJWT());
        }
    }
}
