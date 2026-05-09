
using MediatR;

namespace VolunteerService.Application.Commands.VolunteersEventsCommands.Delete
{
    public class DeleteVolunteersEventsCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
