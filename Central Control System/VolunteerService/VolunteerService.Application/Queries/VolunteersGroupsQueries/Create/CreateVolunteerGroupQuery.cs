using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.DTOs.Create;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.Create
{
    public class CreateVolunteerGroupQuery : IRequest<VolunteersGroupsDTO>
    {
        public required CreateVolunteersGroupsDTO VolunteerGroupDTO { get; set; }
        public string Token { get; set; }
    }

}
