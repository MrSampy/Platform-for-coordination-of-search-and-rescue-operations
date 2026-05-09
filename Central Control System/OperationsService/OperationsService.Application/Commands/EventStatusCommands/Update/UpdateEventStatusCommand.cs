using MediatR;
using OperationsService.Application.DTOs.Update;

namespace OperationsService.Application.Commands.EventStatusCommands.Update
{
    public class UpdateEventStatusCommand : IRequest
    {
        public UpdateEventStatusDTO EventStatus { get; set; }
    }
}
