using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Config;
using OperationsService.API.Model;
using OperationsService.Application.Commands.GroupCommands.Delete;
using OperationsService.Application.Commands.GroupCommands.Update;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Application.Queries.GroupQueries.Create;
using OperationsService.Application.Queries.GroupQueries.GetAll;
using OperationsService.Application.Queries.GroupQueries.GetByGID;
using OperationsService.Domain.Entities;

namespace OperationsService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "Group")]
    public class GroupController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GroupController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetGroups([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllGroupsQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetGroupByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetGroupByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO groupDto)
        {
            return Ok(await _mediator.Send(new CreateGroupQuery() { Group = groupDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupDTO groupModel)
        {
            await _mediator.Send(new UpdateGroupCommand() { Group = groupModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteGroup(Guid gid)
        {
            await _mediator.Send(new DeleteGroupCommand() { GID = gid });
            return NoContent();
        }
    }

}
