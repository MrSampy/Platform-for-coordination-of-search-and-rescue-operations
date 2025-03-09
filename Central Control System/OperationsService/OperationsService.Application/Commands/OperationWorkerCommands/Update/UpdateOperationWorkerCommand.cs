using MediatR;
using OperationsService.Application.DTOs.Update;

namespace OperationsService.Application.Commands.OperationWorkerCommands.Update
{
    public class UpdateOperationWorkerCommand : IRequest
    {
        public required UpdateOperationWorkerDTO OperationWorker { get; set; }
    }
}
