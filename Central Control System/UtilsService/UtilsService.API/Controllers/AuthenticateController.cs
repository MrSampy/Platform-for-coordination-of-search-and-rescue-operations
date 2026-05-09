using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilsService.API.Model;
using UtilsService.Application.DTOs;
using UtilsService.Application.Queries.AuthQueries.GetToken;

namespace UtilsService.API.Controllers
{
    [ApiController]
    [Route("utils/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    public class AuthenticateController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticateController(IMediator mediator) => _mediator = mediator;

        [HttpPost("gettoken")]
        public async Task<IActionResult> GetToken([FromBody] LoginDTO loginDTO, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetTokenQuery() { LoginDTO = loginDTO }, cancellationToken));
        }
    }
}
