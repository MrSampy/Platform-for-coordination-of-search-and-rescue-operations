using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.DTOs.Create;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.IsVolunteerInEvent
{
    public class IsVolunteerinEventQuery : IRequest<IsExistModel>
    {
        public required CreateVolunteersEventsDTO VolunteersEventsDTO { get; set; }
    }
}
