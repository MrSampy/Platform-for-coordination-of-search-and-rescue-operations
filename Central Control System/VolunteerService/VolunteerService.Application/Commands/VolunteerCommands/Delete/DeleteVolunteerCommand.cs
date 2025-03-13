using MediatR;

namespace VolunteerService.Application.Commands.VolunteerCommands.Delete
{
    public class DeleteVolunteerCommand : IRequest
    {
        public required Guid GID { get; set; }
    }

}
