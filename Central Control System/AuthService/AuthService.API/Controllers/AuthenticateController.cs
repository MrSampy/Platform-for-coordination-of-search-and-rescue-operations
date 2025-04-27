using AuthService.API.Config;
using AuthService.API.Core.Interfaces;
using AuthService.API.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateSevice _authenticateSevice;

        public AuthenticateController(IAuthenticateSevice authenticateSevice)
        {
            _authenticateSevice = authenticateSevice;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _authenticateSevice.Login(model, false));
        }


        [HttpPost]
        [Route("2fa/login")]
        public async Task<IActionResult> Login2FA([FromBody] LoginModel model)
        {
            return Ok(await _authenticateSevice.Login(model, true));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            return Ok(await _authenticateSevice.Register(model));
        }


        [HttpPost("key")]
        public async Task<IActionResult> GetAuthenticatorKey([FromBody] LoginModel model)
        {
            return Ok(await _authenticateSevice.GetAuthenticatorKey(model));
        }

        [HttpPost("gettoken")]
        public async Task<IActionResult> GetToken([FromBody] GetTokenRequest request)
        {
            return Ok(await _authenticateSevice.GetToken(request, false));
        }

        [HttpPost("2fa/gettoken")]
        public async Task<IActionResult> GetToken2FA([FromBody] GetTokenRequest request)
        {
            return Ok(await _authenticateSevice.GetToken(request, true));
        }

        [Authorize]
        [HttpGet("me")]
        [RequiresAuthHeader]
        public async Task<IActionResult> Me()
        {
            return Ok(await _authenticateSevice.Me(HttpContext));
        }

        [Authorize]
        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            return Ok(await _authenticateSevice.RegisterAdmin(model));
        }

        [Authorize]
        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-dispatcher")]
        public async Task<IActionResult> RegisterDispatcher([FromBody] RegisterModel model)
        {
            return Ok(await _authenticateSevice.RegisterDispatcher(model));
        }

        [Authorize]
        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-coordinator")]
        public async Task<IActionResult> RegisterCoordinator([FromBody] RegisterModel model)
        {
            return Ok(await _authenticateSevice.RegisterCoordinator(model));
        }
    }
}
