using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.OperationTaskQueries.GetByGID
{
    public class GetOperationTaskByGidQuery : IRequest<OperationTaskDTO>
    {
        public required Guid GID { get; set; }
    }
}
