using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Requests;
using OperationsService.Application.DTOs.Responses;

namespace OperationsService.Application.Queries.MessageQueries.GetAll
{
    public class GetAllMessagesQuery : IRequest<GetAllEntitesReponse<MessageDTO>>
    {
        public required MessagePaginationQuery PaginationQuery { get; set; }
    }
}
