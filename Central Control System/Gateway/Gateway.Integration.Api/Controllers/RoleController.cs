using Gateway.Domain.Services.Interfaces;
using Gateway.Integration.Api.Config;
using Gateway.Integration.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Integration.Api.Controllers
{
    [Authorize]
    [ApiController]
    [RequiresAuthHeader]
    [Route("gateway.integration.api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "Role")]
    public class RoleController : ControllerBase
    {
        private readonly IAuthGateway _authGateway;

        public RoleController(IAuthGateway authGateway)
        {
            _authGateway = authGateway;
        }

        [HttpGet]
        [Route("collection")]
        public IActionResult GetAllRoles(CancellationToken cancellationToken)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(_authGateway.GetAllRoles(cancellationToken, token));
        }
    }
}
