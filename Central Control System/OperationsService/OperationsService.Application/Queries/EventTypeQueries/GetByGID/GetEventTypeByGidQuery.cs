using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.EventTypeQueries.GetByGID
{
    public class GetEventTypeByGidQuery : IRequest<EventTypeDTO>
    {
        public required Guid GID { get; set; }
    }
}
