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

        private SessionEnctyptionService _sessionEncryptionService;

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;

            _sessionEncryptionService = new SessionEnctyptionService();
        }

        [HttpGet]
        public IActionResult GetSessionEncryptionKey()
        {
            return Ok(_sessionEncryptionService.GetPublicEncryptionKey());
        }

        /*
        [HttpPost(Name = "GetSessionToken")]
        public IActionResult GetSessionToken([FromBody]UserDTO data)
        {
            return Ok(_sessionEncryptionService.GetJWT(data));
        }
        */
    }
}
