using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetVolunteersByDistrictGIDQuery
{
    public class GetVolunteersByDistrictGIDQueryQuery : IRequest<IEnumerable<VolunteersDistrictsDTO>>
    {
        public required Guid DistrictGID { get; set; }
    }
}
