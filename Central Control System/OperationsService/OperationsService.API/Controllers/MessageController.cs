using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Config;
using OperationsService.API.Model;
using OperationsService.Application.Commands.MessageCommands.Delete;
using OperationsService.Application.Commands.MessageCommands.Read;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Requests;
using OperationsService.Application.Queries.MessageQueries.Create;
using OperationsService.Application.Queries.MessageQueries.GetAll;
using OperationsService.Application.Queries.MessageQueries.GetByGID;

namespace OperationsService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "Message")]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MessageController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetMessageByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetMessageByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetMessages([FromBody] MessagePaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllMessagesQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDTO eventDto)
        {
            return Ok(await _mediator.Send(new CreateMessageQuery() { Message = eventDto }));
        }

        [HttpPut("read/{gid}")]
        public async Task<IActionResult> ReadMessage(Guid gid)
        {
            await _mediator.Send(new ReadMessageCommand() { GID = gid });

            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteMessage(Guid gid)
        {
            await _mediator.Send(new DeleteMessageCommand() { GID = gid });
            return NoContent();
        }
    }
}
