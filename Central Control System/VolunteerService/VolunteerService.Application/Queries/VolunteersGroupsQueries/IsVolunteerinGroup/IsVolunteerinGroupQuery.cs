using MediatR;
using VolunteerService.Application.DTOs.Create;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.IsVolunteerinGroup
{
    public class IsVolunteerinGroupQuery : IRequest<bool>
    {
        public required CreateVolunteersGroupsDTO VolunteersGroupsDTO { get; set; }
    }
}
