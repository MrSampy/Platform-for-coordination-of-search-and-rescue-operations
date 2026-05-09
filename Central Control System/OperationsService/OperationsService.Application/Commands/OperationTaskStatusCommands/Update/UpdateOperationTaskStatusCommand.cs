using MediatR;
using OperationsService.Application.DTOs.Update;

namespace OperationsService.Application.Commands.OperationTaskStatusCommands.Update
{
    public class UpdateOperationTaskStatusCommand : IRequest
    {
        public required UpdateOperationTaskStatusDTO OperationTaskStatus { get; set; }
    }
}
