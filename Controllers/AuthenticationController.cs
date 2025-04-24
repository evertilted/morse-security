using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using morse_auth.Database;
using morse_auth.DTO;
using morse_auth.Services;

namespace morse_auth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        private AuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IDbContextFactory<MSDBContext> contextFactory)
        {
            _logger = logger;

            _authenticationService = new AuthenticationService(contextFactory);
        }

        [HttpPost(Name = "Register")]
        public IActionResult Register([FromBody]AuthUserDTO data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(_authenticationService.Register(data));
            }
            catch (DbUpdateException)
            {
                return BadRequest($"User with such login ({data.Login}) already exists");
            }
        }

        [HttpPost(Name = "Login")]
        public IActionResult Login([FromBody]AuthUserDTO data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(_authenticationService.Login(data));
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
