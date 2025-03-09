using MediatR;

namespace OperationsService.Application.Commands.EventTypeCommands.Delete
{
    public class DeleteEventTypeCommand : IRequest
    {
        public required Guid GID { get; set; }
    }

}
