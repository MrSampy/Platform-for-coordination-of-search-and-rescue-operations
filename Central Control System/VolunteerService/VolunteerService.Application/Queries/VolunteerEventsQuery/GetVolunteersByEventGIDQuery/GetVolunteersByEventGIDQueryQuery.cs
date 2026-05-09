using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.GetVolunteersByEventGIDQuery
{
    public class GetVolunteersByEventGIDQueryQuery : IRequest<IEnumerable<VolunteersEventsDTO>>
    {
        public required Guid EventGID { get; set; }
    }
}
