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
    [ApiExplorerSettings(GroupName = "User")]
    public class UserController : ControllerBase
    {
        private readonly IAuthGateway _authGateway;
        private readonly IAuthService _authService;

        public UserController(IAuthGateway authGateway, IAuthService authService)
        {
            _authGateway = authGateway;
            _authService = authService;
        }

        [RequiresAuthHeader]
        [Authorize]
        [HttpGet]
        [Route("collection")]
        public IActionResult GetAllUsers(CancellationToken cancellationToken)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(_authGateway.GetAllUsers(cancellationToken, token));
        }

        [HttpGet("byname/{userName}")]
        public IActionResult GetByUserName([FromRoute] string userName, CancellationToken cancellationToken)
        {
            return Ok(_authService.IsUserWithSuchName(userName, cancellationToken));
        }

        [HttpGet("byemail/{email}")]
        public IActionResult GetByEmail([FromRoute] string email, CancellationToken cancellationToken)
        {
            return Ok(_authService.IsUserWithSuchEmail(email, cancellationToken));
        }

        [RequiresAuthHeader]
        [Authorize]
        [HttpGet("bygid/{gid}")]
        public IActionResult GetByGID([FromRoute] Guid gid, CancellationToken cancellationToken)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(_authGateway.GetByGID(gid, cancellationToken, token));
        }

        [RequiresAuthHeader]
        [Authorize]
        [HttpGet]
        [Route("{roleName}")]
        public IActionResult GetAllUsers([FromRoute] string roleName, CancellationToken cancellationToken)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(_authGateway.GetAllUserIdsByRole(roleName, cancellationToken, token));
        }

        [RequiresAuthHeader]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserRoles(UserDTO query)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _authGateway.UpdateUserRoles(query, token));
        }
    }
}
