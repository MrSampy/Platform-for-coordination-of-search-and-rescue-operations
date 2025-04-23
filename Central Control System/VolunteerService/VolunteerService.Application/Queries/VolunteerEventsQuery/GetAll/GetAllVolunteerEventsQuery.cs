using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.GetAll
{
    public class GetAllVolunteerEventsQuery : IRequest<IEnumerable<VolunteersEventsDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
