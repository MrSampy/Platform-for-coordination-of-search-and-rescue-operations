using MediatR;

namespace VolunteerService.Application.Commands.VolunteersDistrictsCommands.Delete
{
    public class DeleteVolunteersDistrictCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
