using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Config;
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
    [Authorize]
    [RequiresAuthHeader]
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
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _mediator.Send(new CreateOperationWorkerQuery() { OperationWorker = operationWorkerDto, Token = token }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOperationWorker([FromBody] UpdateOperationWorkerDTO operationWorkerModel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            await _mediator.Send(new UpdateOperationWorkerCommand() { OperationWorker = operationWorkerModel, Token = token });
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
