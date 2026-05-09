using MediatR;

namespace OperationsService.Application.Commands.MessageCommands.Delete
{
    public class DeleteMessageCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
