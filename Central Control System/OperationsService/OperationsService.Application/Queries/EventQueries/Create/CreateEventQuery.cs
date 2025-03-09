using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.EventQueries.Create
{
    public class CreateEventQuery : IRequest<EventDTO>
    {
        public required CreateEventDTO Event { get; set; }
    }
}
