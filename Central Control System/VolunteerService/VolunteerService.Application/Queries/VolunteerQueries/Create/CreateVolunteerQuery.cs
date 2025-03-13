using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.DTOs.Create;

namespace VolunteerService.Application.Queries.VolunteerQueries.Create
{
    public class CreateVolunteerQuery : IRequest<VolunteerDTO>
    {
        public required CreateVolunteerDTO VolunteerDTO { get; set; }
    }
}
