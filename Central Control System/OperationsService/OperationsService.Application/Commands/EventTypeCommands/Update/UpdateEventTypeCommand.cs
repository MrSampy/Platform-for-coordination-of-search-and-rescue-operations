using MediatR;
using OperationsService.Application.DTOs.Update;

namespace OperationsService.Application.Commands.EventTypeCommands.Update
{
    public class UpdateEventTypeCommand : IRequest
    {
        public UpdateEventTypeDTO EventType { get; set; }
    }
}
