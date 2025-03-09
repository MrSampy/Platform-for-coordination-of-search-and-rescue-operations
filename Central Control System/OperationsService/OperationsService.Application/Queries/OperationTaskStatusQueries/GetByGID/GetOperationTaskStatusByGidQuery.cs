using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.OperationTaskStatusQueries.GetByGID
{
    public class GetOperationTaskStatusByGidQuery : IRequest<OperationTaskStatusDTO>
    {
        public required Guid GID { get; set; }
    }
}
