using MediatR;

namespace OperationsService.Application.Commands.OperationTaskStatusCommands.Delete
{
    public class DeleteOperationTaskStatusCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
