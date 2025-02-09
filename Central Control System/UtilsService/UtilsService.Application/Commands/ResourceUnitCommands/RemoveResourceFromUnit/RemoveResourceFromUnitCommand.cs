using MediatR;

namespace UtilsService.Application.Commands.ResourceUnitCommands.RemoveResourceFromUnit
{
    public class RemoveResourceFromUnitCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
