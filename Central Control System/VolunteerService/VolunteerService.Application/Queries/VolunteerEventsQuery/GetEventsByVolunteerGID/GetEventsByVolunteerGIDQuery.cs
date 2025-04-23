using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.GetEventsByVolunteerGID
{
    public class GetEventsByVolunteerGIDQuery : IRequest<IEnumerable<VolunteersEventsDTO>>
    {
        public required Guid VolunteerGID { get; set; }
    }
}
