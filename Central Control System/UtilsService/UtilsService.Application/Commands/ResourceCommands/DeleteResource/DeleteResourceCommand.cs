using MediatR;

namespace UtilsService.Application.Commands.ResourceCommands.DeleteResource
{
    public class DeleteResourceCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
