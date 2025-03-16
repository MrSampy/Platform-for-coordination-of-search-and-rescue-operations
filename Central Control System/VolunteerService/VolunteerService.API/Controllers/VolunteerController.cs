using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerService.API.Config;
using VolunteerService.API.Model;
using VolunteerService.Application.Commands.VolunteerCommands.Delete;
using VolunteerService.Application.Commands.VolunteerCommands.Update;
using VolunteerService.Application.DTOs.Create;
using VolunteerService.Application.DTOs.Update;
using VolunteerService.Application.Queries.VolunteerQueries.Create;
using VolunteerService.Application.Queries.VolunteerQueries.GetAll;
using VolunteerService.Application.Queries.VolunteerQueries.GetByGID;
using VolunteerService.Domain.Entities;

namespace VolunteerService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("volunteers/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "Volunteer")]
    public class VolunteerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VolunteerController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetVolunteers([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllVolunteersQuery { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteerByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetVolunteerByGidQuery { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateVolunteer([FromBody] CreateVolunteerDTO volunteerDto)
        {
            return Ok(await _mediator.Send(new CreateVolunteerQuery { VolunteerDTO = volunteerDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVolunteer([FromBody] UpdateVolunteerDTO volunteerModel)
        {
            await _mediator.Send(new UpdateVolunteerCommand { VolunteerDTO = volunteerModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteVolunteer(Guid gid)
        {
            await _mediator.Send(new DeleteVolunteerCommand { GID = gid });
            return NoContent();
        }
    }
}
