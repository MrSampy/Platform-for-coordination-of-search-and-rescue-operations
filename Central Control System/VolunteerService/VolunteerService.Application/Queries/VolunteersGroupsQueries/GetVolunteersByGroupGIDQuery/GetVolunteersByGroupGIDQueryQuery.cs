using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.GetVolunteersByGroupGIDQuery
{
    public class GetVolunteersByGroupGIDQueryQuery : IRequest<IEnumerable<VolunteersGroupsDTO>>
    {
        public required Guid GroupGID { get; set; }
    }
}
