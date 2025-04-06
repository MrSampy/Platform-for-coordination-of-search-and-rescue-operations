using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Auth;
using Gateway.Integration.Api.Config;
using Gateway.Integration.Api.Model;
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

        public AuthenticateController(IAuthGateway authGateway)
        {
            _authGateway = authGateway;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            return Ok(await _authGateway.Register(model));
        }

        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _authGateway.RegisterAdmin(model, token));
        }

        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-dispatcher")]
        public async Task<IActionResult> RegisterDispatcher([FromBody] RegisterModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _authGateway.RegisterDispatcher(model, token));
        }

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
