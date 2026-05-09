using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.MessageQueries.Create
{
    public class CreateMessageQuery : IRequest<MessageDTO>
    {
        public required CreateMessageDTO Message { get; set; }
    }
}
