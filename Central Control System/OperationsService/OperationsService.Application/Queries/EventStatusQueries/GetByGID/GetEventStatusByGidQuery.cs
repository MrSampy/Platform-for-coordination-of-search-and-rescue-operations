using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.EventStatusQueries.GetByGID
{
    public class GetEventStatusByGidQuery : IRequest<EventStatusDTO>
    {
        public required Guid GID { get; set; }
    }
}
