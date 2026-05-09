using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Queries.EventStatusQueries.GetAll
{
    public class GetAllEventStatusesQuery : IRequest<IEnumerable<EventStatusDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
