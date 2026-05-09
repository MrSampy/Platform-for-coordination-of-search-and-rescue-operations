using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.OperationTaskStatusQueries.Create
{
    public class CreateOperationTaskStatusQuery : IRequest<OperationTaskStatusDTO>
    {
        public required CreateOperationTaskStatusDTO OperationTaskStatus { get; set; }
    }
}
