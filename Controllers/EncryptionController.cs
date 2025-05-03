using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using morse_auth.DTO;
using morse_auth.Services;

namespace morse_auth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EncryptionController : ControllerBase
    {
        private readonly ILogger<EncryptionController> _logger;

        private EncryptionService _encryptionService;

        public EncryptionController(ILogger<EncryptionController> logger)
        {
            _logger = logger;

            _encryptionService = new EncryptionService();
        }

        /// <summary>
        /// Returns the public key that the client will use to encrypt data before sending to server
        /// </summary>
        /// <returns>The public encryption key</returns>
        [HttpGet(Name = "clientEncryptionKey")]
        public IActionResult ClientEncryptionKey()
        {
            return Ok(_encryptionService.GetPublicEncryptionKey());
        }
    }
}
