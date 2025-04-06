using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Auth;
using Gateway.Integration.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Integration.Api.Controllers
{
    [ApiController]
    [Route("gateway.integration.api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    public class TokenController : ControllerBase
    {
        private readonly IAuthGateway _authGateway;

        public TokenController(IAuthGateway authGateway)
        {
            _authGateway = authGateway;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _authGateway.Login(model));
        }
    }
}
