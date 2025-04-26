using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Requests;
using OperationsService.Application.DTOs.Responses;

namespace OperationsService.Application.Queries.GroupQueries.GetAllSort
{
    public class GetAllGroupsSortQuery : IRequest<GetAllEntitesReponse<GroupDTO>>
    {
        public required GroupPaginationQuery PaginationQuery { get; set; }
    }
}
