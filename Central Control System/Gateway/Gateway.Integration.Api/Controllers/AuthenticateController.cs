using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Auth;
using Gateway.Integration.Api.Config;
using Gateway.Integration.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Integration.Api.Controllers
{
    [ApiController]
    [Route("gateway.integration.api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "Authenticate")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthGateway _authGateway;
        private readonly IAuthService _authService;

        public AuthenticateController(IAuthGateway authGateway, IAuthService authService)
        {
            _authGateway = authGateway;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            return Ok(await _authGateway.Register(model));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _authGateway.Login(model));
        }

        [HttpPost]
        [Route("2fa/login")]
        public async Task<IActionResult> Login2FA([FromBody] LoginModel model)
        {
            return Ok(await _authGateway.Login2FA(model));
        }


        [HttpPost("key")]
        public async Task<IActionResult> GetAuthenticatorKey([FromBody] LoginModel model)
        {
            return Ok(await _authGateway.GetAuthenticatorKey(model));
        }

        [HttpPost("gettoken")]
        public async Task<IActionResult> GetToken([FromBody] GetTokenRequest request)
        {
            return Ok(await _authGateway.GetToken(request));
        }

        [HttpPost("2fa/gettoken")]
        public async Task<IActionResult> GetToken2FA([FromBody] GetTokenRequest request)
        {
            return Ok(await _authGateway.GetToken2FA(request));
        }

        [Authorize]
        [HttpGet]
        [RequiresAuthHeader]
        [Route("me")]
        public async Task<IActionResult> Me()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _authGateway.Me(token));
        }

        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _authGateway.RegisterAdmin(model, token));
        }

        [Authorize]
        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-worker")]
        public async Task<IActionResult> RegisterWorker([FromBody] RegisterWorkerModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _authService.RegisterWorker(model, token);
            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-dispatcher")]
        public async Task<IActionResult> RegisterDispatcher([FromBody] RegisterModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _authGateway.RegisterDispatcher(model, token));
        }

        [Authorize]
        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-coordinator")]
        public async Task<IActionResult> RegisterCoordinator([FromBody] RegisterModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _authGateway.RegisterCoordinator(model, token));
        }
    }
}
