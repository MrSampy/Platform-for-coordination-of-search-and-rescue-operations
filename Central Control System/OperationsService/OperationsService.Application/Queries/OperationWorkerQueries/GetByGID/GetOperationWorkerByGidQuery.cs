using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.OperationWorkerQueries.GetByGID
{
    public class GetOperationWorkerByGidQuery : IRequest<OperationWorkerDTO>
    {
        public required Guid GID { get; set; }
    }
}
