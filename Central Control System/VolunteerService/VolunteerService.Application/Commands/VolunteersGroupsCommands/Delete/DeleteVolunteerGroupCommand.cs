
using MediatR;

namespace VolunteerService.Application.Commands.VolunteersGroupsCommands.Delete
{
    public class DeleteVolunteerGroupCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
