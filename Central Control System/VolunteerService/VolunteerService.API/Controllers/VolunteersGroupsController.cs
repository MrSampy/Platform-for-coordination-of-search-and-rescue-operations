using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerService.API.Config;
using VolunteerService.API.Model;
using VolunteerService.Application.Commands.VolunteersGroupsCommands.Delete;
using VolunteerService.Application.DTOs.Create;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.Create;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetAll;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetByGID;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetGroupsByVolunteerGID;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetVolunteersByGroupGIDQuery;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.IsVolunteerinGroup;
using VolunteerService.Domain.Entities;

namespace VolunteerService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("volunteers/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "VolunteersGroups")]
    public class VolunteersGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VolunteersGroupsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetVolunteersGroups([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllVolunteerGroupsQuery { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("by-volunteer/{volunteerGid}")]
        public async Task<IActionResult> GetGroupsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetGroupsByVolunteerGIDQuery { VolunteerGID = volunteerGid }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("by-group/{groupGid}")]
        public async Task<IActionResult> GetVolunteersByGroupGID(Guid groupGid, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetVolunteersByGroupGIDQueryQuery { GroupGID = groupGid }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteersGroupsByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetVolunteersGroupsByGidQuery { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> AddVolunteerToGroup([FromBody] CreateVolunteersGroupsDTO volunteersGroups)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _mediator.Send(new CreateVolunteerGroupQuery { VolunteerGroupDTO = volunteersGroups, Token = token }));
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> RemoveVolunteerFromGroup(Guid gid)
        {
            await _mediator.Send(new DeleteVolunteerGroupCommand { GID = gid });
            return NoContent();
        }

        [HttpPost("exists")]
        public async Task<IActionResult> IsVolunteerinGroup([FromBody] CreateVolunteersGroupsDTO volunteersGroupsDTO, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new IsVolunteerinGroupQuery { VolunteersGroupsDTO = volunteersGroupsDTO }, cancellationToken);
            return Ok(result);
        }
    }
}
