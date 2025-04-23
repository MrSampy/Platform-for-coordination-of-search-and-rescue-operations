using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.GetByGID
{
    public class GetVolunteersEventsByGidQuery : IRequest<VolunteersEventsDTO>
    {
        public required Guid GID { get; set; }

    }
}
