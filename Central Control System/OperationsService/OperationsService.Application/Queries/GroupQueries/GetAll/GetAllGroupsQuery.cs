using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Queries.GroupQueries.GetAll
{
    public class GetAllGroupsQuery : IRequest<IEnumerable<GroupDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }

}
