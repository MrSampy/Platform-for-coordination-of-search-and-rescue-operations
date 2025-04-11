using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.EventQueries.GetByStatusGID
{
    public class GetByStatusGIDQuery : IRequest<IEnumerable<EventDTO>>
    {
        public required Guid EventStatusGID { get; set; }
    }
}
