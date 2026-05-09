using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.GroupQueries.Create
{
    public class CreateGroupQuery : IRequest<GroupDTO>
    {
        public required CreateGroupDTO Group { get; set; }
        public string Token { get; set; }
    }
}
