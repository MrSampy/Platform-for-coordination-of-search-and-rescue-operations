using MediatR;

namespace OperationsService.Application.Commands.MessageCommands.Read
{
    public class ReadMessageCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
