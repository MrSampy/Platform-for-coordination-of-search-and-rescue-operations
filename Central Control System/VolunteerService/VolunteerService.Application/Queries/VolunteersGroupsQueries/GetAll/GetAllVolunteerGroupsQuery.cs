using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.GetAll
{
    public class GetAllVolunteerGroupsQuery : IRequest<IEnumerable<VolunteersGroupsDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
