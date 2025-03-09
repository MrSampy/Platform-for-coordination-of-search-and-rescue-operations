using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.EventStatusQueries.Create
{
    public class CreateEventStatusQuery : IRequest<EventStatusDTO>
    {
        public required CreateEventStatusDTO EventStatus { get; set; }
    }
}
