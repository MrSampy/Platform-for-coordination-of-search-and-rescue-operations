using MediatR;
using OperationsService.Application.DTOs.Update;

namespace OperationsService.Application.Commands.OperationTaskCommands.Update
{
    public class UpdateOperationTaskCommand : IRequest
    {
        public UpdateOperationTaskDTO OperationTask { get; set; }
    }
}
