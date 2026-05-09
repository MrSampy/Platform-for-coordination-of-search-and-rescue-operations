using MediatR;

namespace OperationsService.Application.Commands.OperationWorkerCommands.Delete
{
    public class DeleteOperationWorkerCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
