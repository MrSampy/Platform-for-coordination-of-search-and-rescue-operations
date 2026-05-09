using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.EventQueries.GetByGID
{
    public class GetEventByGidQuery : IRequest<EventDTO>
    {
        public required Guid GID { get; set; }
    }
}
