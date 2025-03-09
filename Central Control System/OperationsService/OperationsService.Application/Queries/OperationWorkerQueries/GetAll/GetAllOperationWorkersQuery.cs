using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Queries.OperationWorkerQueries.GetAll
{
    public class GetAllOperationWorkersQuery : IRequest<IEnumerable<OperationWorkerDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
