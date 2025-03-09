using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.OperationWorkerQueries.Create
{
    public class CreateOperationWorkerQuery : IRequest<OperationWorkerDTO>
    {
        public required CreateOperationWorkerDTO OperationWorker { get; set; }
    }

}
