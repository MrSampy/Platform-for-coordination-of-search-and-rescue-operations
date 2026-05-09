using MediatR;

namespace OperationsService.Application.Commands.ResourcesEventCommands.Delete
{
    public class DeleteResourcesEventCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
