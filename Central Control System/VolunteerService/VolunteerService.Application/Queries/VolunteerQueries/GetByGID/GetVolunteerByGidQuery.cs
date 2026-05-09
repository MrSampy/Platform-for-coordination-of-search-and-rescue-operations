using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteerQueries.GetByGID
{
    public class GetVolunteerByGidQuery : IRequest<VolunteerDTO>
    {
        public required Guid GID { get; set; }
    }
}
