using MediatR;
using Microsoft.AspNetCore.Mvc;
using VolunteerService.API.Model;
using VolunteerService.Application.Commands.VolunteersGroupsCommands.Delete;
using VolunteerService.Application.Commands.VolunteersGroupsCommands.Update;
using VolunteerService.Application.DTOs.Create;
using VolunteerService.Application.DTOs.Update;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.Create;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetAll;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetByGID;
using VolunteerService.Domain.Entities;

namespace VolunteerService.API.Controllers
{
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

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteersGroupByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetVolunteersGroupsByGidQuery { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateVolunteersGroup([FromBody] CreateVolunteersGroupsDTO groupDto)
        {
            return Ok(await _mediator.Send(new CreateVolunteerGroupQuery { VolunteerGroupDTO = groupDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVolunteersGroup([FromBody] UpdateVolunteersGroupsDTO groupModel)
        {
            await _mediator.Send(new UpdateVolunteerGroupCommand { VolunteerGroupDTO = groupModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteVolunteersGroup(Guid gid)
        {
            await _mediator.Send(new DeleteVolunteerGroupCommand { GID = gid });
            return NoContent();
        }
    }
}
