using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.OperationTaskQueries.Create
{
    public class CreateOperationTaskQuery : IRequest<OperationTaskDTO>
    {
        public required CreateOperationTaskDTO OperationTask { get; set; }
    }
}
