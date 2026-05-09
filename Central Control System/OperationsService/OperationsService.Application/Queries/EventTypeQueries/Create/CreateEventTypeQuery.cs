using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.EventTypeQueries.Create
{
    public class CreateEventTypeQuery : IRequest<EventTypeDTO>
    {
        public required CreateEventTypeDTO EventType { get; set; }
    }
}
