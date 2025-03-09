using MediatR;

namespace OperationsService.Application.Commands.EventStatusCommands.Delete
{
    public class DeleteEventStatusCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
