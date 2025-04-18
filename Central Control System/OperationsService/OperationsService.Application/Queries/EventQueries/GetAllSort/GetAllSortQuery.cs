using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Requests;
using OperationsService.Application.DTOs.Responses;

namespace OperationsService.Application.Queries.EventQueries.GetAllSort
{
    public class GetAllSortQuery : IRequest<GetAllEntitesReponse<EventDTO>>
    {
        public required EventPaginationQuery PaginationQuery { get; set; }
    }
}
