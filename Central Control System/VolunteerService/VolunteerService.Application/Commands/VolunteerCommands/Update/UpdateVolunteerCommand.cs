using MediatR;
using VolunteerService.Application.DTOs.Update;

namespace VolunteerService.Application.Commands.VolunteerCommands.Update
{
    public class UpdateVolunteerCommand : IRequest
    {
        public UpdateVolunteerDTO VolunteerDTO { get; set; }
    }

}
