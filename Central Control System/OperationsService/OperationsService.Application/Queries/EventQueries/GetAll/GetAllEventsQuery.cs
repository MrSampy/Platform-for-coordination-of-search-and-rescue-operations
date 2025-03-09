using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Queries.EventQueries.GetAll
{
    public class GetAllEventsQuery : IRequest<IEnumerable<EventDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
