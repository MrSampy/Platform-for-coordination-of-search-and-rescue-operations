using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.MessageQueries.GetByGID
{
    public class GetMessageByGidQuery : IRequest<MessageDTO>
    {
        public required Guid GID { get; set; }
    }
}
