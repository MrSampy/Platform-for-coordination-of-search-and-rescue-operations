using MediatR;
using VolunteerService.Application.DTOs.Update;

namespace VolunteerService.Application.Commands.VolunteersGroupsCommands.Update
{
    public class UpdateVolunteerGroupCommand : IRequest
    {
        public UpdateVolunteersGroupsDTO VolunteerGroupDTO { get; set; }
        public string Token { get; set; }
    }
}
