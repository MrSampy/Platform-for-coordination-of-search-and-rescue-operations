using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.GroupQueries.GetByGID
{
    public class GetGroupByGidQuery : IRequest<GroupDTO>
    {
        public required Guid GID { get; set; }
    }
}
