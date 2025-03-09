using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Queries.OperationTaskStatusQueries.GetAll
{
    public class GetAllOperationTaskStatusesQuery : IRequest<IEnumerable<OperationTaskStatusDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }

}
