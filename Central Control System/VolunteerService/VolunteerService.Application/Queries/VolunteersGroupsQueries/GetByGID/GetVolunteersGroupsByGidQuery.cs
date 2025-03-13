using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.GetByGID
{
    public class GetVolunteersGroupsByGidQuery : IRequest<VolunteersGroupsDTO>
    {
        public required Guid GID { get; set; }

    }
}
