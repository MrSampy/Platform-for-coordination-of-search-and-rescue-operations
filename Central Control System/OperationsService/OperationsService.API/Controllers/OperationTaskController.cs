using MediatR;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Model;
using OperationsService.Application.Commands.OperationTaskCommands.Delete;
using OperationsService.Application.Commands.OperationTaskCommands.Update;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Application.Queries.OperationTaskQueries.Create;
using OperationsService.Application.Queries.OperationTaskQueries.GetAll;
using OperationsService.Application.Queries.OperationTaskQueries.GetByGID;
using OperationsService.Domain.Entities;

namespace OperationsService.API.Controllers
{
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "OperationTask")]
    public class OperationTaskController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OperationTaskController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetOperationTasks([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllOperationTasksQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetOperationTaskByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetOperationTaskByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOperationTask([FromBody] CreateOperationTaskDTO operationTaskDto)
        {
            return Ok(await _mediator.Send(new CreateOperationTaskQuery() { OperationTask = operationTaskDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOperationTask([FromBody] UpdateOperationTaskDTO operationTaskModel)
        {
            await _mediator.Send(new UpdateOperationTaskCommand() { OperationTask = operationTaskModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteOperationTask(Guid gid)
        {
            await _mediator.Send(new DeleteOperationTaskCommand() { GID = gid });
            return NoContent();
        }
    }

}
