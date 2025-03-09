using MediatR;

namespace OperationsService.Application.Commands.GroupCommands.Delete
{
    public class DeleteGroupCommand : IRequest
    {
        public required Guid GID { get; set; }
    }

}
