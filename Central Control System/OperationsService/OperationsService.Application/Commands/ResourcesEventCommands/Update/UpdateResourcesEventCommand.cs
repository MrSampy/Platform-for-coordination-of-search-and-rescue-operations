using MediatR;
using OperationsService.Application.DTOs.Update;

namespace OperationsService.Application.Commands.ResourcesEventCommands.Update
{
    public class UpdateResourcesEventCommand : IRequest
    {
        public required UpdateResourcesEventDTO ResourcesEvent { get; set; }
    }

}
