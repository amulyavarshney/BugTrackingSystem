using BugTrackingSystem.Services;
using BugTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingSystem.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginView)
        {
            return Ok(await _authService.Login(loginView));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateViewModel registerview)
        {
            await _authService.Register(registerview);
            return Ok();
        }
    }
}
