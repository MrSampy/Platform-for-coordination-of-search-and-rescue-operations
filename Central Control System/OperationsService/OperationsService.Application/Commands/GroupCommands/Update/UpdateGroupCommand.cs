using MediatR;
using OperationsService.Application.DTOs.Update;

namespace OperationsService.Application.Commands.GroupCommands.Update
{
    public class UpdateGroupCommand : IRequest
    {
        public UpdateGroupDTO Group { get; set; }
    }
}
