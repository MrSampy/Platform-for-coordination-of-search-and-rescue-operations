using MediatR;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Model;
using OperationsService.Application.Commands.OperationWorkerCommands.Delete;
using OperationsService.Application.Commands.OperationWorkerCommands.Update;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Application.Queries.OperationWorkerQueries.Create;
using OperationsService.Application.Queries.OperationWorkerQueries.GetAll;
using OperationsService.Application.Queries.OperationWorkerQueries.GetByGID;
using OperationsService.Domain.Entities;

namespace OperationsService.API.Controllers
{
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "OperationWorker")]
    public class OperationWorkerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OperationWorkerController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetOperationWorkers([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllOperationWorkersQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetOperationWorkerByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetOperationWorkerByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOperationWorker([FromBody] CreateOperationWorkerDTO operationWorkerDto)
        {
            return Ok(await _mediator.Send(new CreateOperationWorkerQuery() { OperationWorker = operationWorkerDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOperationWorker([FromBody] UpdateOperationWorkerDTO operationWorkerModel)
        {
            await _mediator.Send(new UpdateOperationWorkerCommand() { OperationWorker = operationWorkerModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteOperationWorker(Guid gid)
        {
            await _mediator.Send(new DeleteOperationWorkerCommand() { GID = gid });
            return NoContent();
        }
    }

}
