using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService
            )
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest req)
        {
            var result  = await _authService.LoginAsync(req);

            if (!result.IsSuccessful)
                return Unauthorized(new { result.Errors });

            return Ok(new { result.Token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest req)
        {
            var result = await _authService.RegisterAsync(req);

            if (!result.IsSuccessful)
                return BadRequest(result.Errors);

            return Ok( new { result.Token });
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("hui");
        }
    }
}
