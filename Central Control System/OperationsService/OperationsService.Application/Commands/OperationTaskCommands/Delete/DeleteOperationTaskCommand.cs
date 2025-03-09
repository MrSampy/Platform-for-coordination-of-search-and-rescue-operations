using MediatR;

namespace OperationsService.Application.Commands.OperationTaskCommands.Delete
{
    public class DeleteOperationTaskCommand : IRequest
    {
        public required Guid GID { get; set; }
    }

}
