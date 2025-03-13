using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetAll
{
    public class GetAllVolunteersDistrictsQuery : IRequest<IEnumerable<VolunteersDistrictsDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
