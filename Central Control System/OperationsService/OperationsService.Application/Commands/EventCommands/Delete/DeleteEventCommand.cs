using MediatR;

namespace OperationsService.Application.Commands.EventCommands.Delete
{
    public class DeleteEventCommand : IRequest
    {
        public required Guid GID { get; set; }
    }

}
