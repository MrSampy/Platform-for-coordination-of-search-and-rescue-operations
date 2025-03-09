using MediatR;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Model;
using OperationsService.Application.Commands.OperationTaskStatusCommands.Delete;
using OperationsService.Application.Commands.OperationTaskStatusCommands.Update;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Application.Queries.OperationTaskStatusQueries.Create;
using OperationsService.Application.Queries.OperationTaskStatusQueries.GetAll;
using OperationsService.Application.Queries.OperationTaskStatusQueries.GetByGID;
using OperationsService.Domain.Entities;

namespace OperationsService.API.Controllers
{
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "OperationTaskStatus")]
    public class OperationTaskStatusController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OperationTaskStatusController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetOperationTaskStatuses([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllOperationTaskStatusesQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetOperationTaskStatusByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetOperationTaskStatusByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOperationTaskStatus([FromBody] CreateOperationTaskStatusDTO operationTaskStatusDto)
        {
            return Ok(await _mediator.Send(new CreateOperationTaskStatusQuery() { OperationTaskStatus = operationTaskStatusDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOperationTaskStatus([FromBody] UpdateOperationTaskStatusDTO operationTaskStatusModel)
        {
            await _mediator.Send(new UpdateOperationTaskStatusCommand() { OperationTaskStatus = operationTaskStatusModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteOperationTaskStatus(Guid gid)
        {
            await _mediator.Send(new DeleteOperationTaskStatusCommand() { GID = gid });
            return NoContent();
        }
    }

}
