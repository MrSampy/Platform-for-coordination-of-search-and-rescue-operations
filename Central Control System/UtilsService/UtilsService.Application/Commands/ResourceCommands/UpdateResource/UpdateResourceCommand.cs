using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Commands.ResourceCommands.UpdateResource
{
    public class UpdateResourceCommand : IRequest
    {
        public required Resource Resource { get; set; }
    }
}
