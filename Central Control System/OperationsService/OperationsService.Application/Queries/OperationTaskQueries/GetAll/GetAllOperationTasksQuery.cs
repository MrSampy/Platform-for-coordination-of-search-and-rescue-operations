using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Queries.OperationTaskQueries.GetAll
{
    public class GetAllOperationTasksQuery : IRequest<IEnumerable<OperationTaskDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }

}
