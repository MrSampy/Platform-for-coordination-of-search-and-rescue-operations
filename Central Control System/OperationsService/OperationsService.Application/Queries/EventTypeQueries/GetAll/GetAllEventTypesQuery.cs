using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Queries.EventTypeQueries.GetAll
{
    public class GetAllEventTypesQuery : IRequest<IEnumerable<EventTypeDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
