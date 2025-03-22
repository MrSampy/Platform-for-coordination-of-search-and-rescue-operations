using MediatR;
using VolunteerService.Application.DTOs.Update;

namespace VolunteerService.Application.Commands.VolunteersDistrictsCommands.Update
{
    public class UpdateVolunteersDistrictCommand : IRequest
    {
        public required UpdateVolunteersDistrictsDTO VolunteersDistrictDTO { get; set; }
        public string Token { get; set; }
    }

}
