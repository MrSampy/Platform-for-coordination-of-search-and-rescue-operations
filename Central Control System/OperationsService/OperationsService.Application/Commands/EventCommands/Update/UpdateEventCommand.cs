using MediatR;
using OperationsService.Application.DTOs.Update;

namespace OperationsService.Application.Commands.EventCommands.Update
{
    public class UpdateEventCommand : IRequest
    {
        public UpdateEventDTO Event { get; set; }
    }

}
