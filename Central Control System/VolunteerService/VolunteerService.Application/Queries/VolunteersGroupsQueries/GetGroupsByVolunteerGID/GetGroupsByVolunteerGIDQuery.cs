using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.GetGroupsByVolunteerGID
{
    public class GetGroupsByVolunteerGIDQuery : IRequest<IEnumerable<VolunteersGroupsDTO>>
    {
        public required Guid VolunteerGID { get; set; }
    }
}
