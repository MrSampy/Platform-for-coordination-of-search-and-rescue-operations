using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerService.API.Config;
using VolunteerService.API.Model;
using VolunteerService.Application.Commands.VolunteersEventsCommands.Delete;
using VolunteerService.Application.DTOs.Create;
using VolunteerService.Application.Queries.VolunteerEventsQuery.Create;
using VolunteerService.Application.Queries.VolunteerEventsQuery.GetAll;
using VolunteerService.Application.Queries.VolunteerEventsQuery.GetByGID;
using VolunteerService.Application.Queries.VolunteerEventsQuery.GetEventsByVolunteerGID;
using VolunteerService.Application.Queries.VolunteerEventsQuery.GetVolunteersByEventGIDQuery;
using VolunteerService.Application.Queries.VolunteerEventsQuery.IsVolunteerInEvent;
using VolunteerService.Domain.Entities;

namespace VolunteerService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("volunteers/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "VolunteersEvents")]
    public class VolunteersEventsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VolunteersEventsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetVolunteersEvents([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllVolunteerEventsQuery { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("by-volunteer/{volunteerGid}")]
        public async Task<IActionResult> GetEventsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetEventsByVolunteerGIDQuery { VolunteerGID = volunteerGid }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("by-event/{EventGid}")]
        public async Task<IActionResult> GetVolunteersByEventGID(Guid EventGid, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetVolunteersByEventGIDQueryQuery { EventGID = EventGid }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteersEventsByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetVolunteersEventsByGidQuery { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> AddVolunteerToEvent([FromBody] CreateVolunteersEventsDTO volunteersEvents)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _mediator.Send(new CreateVolunteerEventsQuery { VolunteerEventsDTO = volunteersEvents, Token = token }));
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> RemoveVolunteerFromEvent(Guid gid)
        {
            await _mediator.Send(new DeleteVolunteersEventsCommand { GID = gid });
            return NoContent();
        }

        [HttpPost("exists")]
        public async Task<IActionResult> IsVolunteerinEvent([FromBody] CreateVolunteersEventsDTO volunteersEventsDTO, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new IsVolunteerinEventQuery { VolunteersEventsDTO = volunteersEventsDTO }, cancellationToken);
            return Ok(result);
        }
    }
}
