using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.DTOs.Create;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.Create
{
    public class CreateVolunteerEventsQuery : IRequest<VolunteersEventsDTO>
    {
        public required CreateVolunteersEventsDTO VolunteerEventsDTO { get; set; }
        public string Token { get; set; }
    }

}
