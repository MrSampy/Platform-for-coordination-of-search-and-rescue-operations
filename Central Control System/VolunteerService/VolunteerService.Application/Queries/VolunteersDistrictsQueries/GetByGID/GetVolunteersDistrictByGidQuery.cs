using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetByGID
{
    public class GetVolunteersDistrictByGidQuery : IRequest<VolunteersDistrictsDTO>
    {
        public required Guid GID { get; set; }
    }
}
