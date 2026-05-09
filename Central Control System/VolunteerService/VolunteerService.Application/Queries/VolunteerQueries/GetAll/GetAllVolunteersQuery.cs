using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;

namespace VolunteerService.Application.Queries.VolunteerQueries.GetAll
{
    public class GetAllVolunteersQuery : IRequest<IEnumerable<VolunteerDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
