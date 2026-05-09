using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetDistrictsByVolunteerGID
{
    public class GetDistrictsByVolunteerGIDQuery : IRequest<IEnumerable<VolunteersDistrictsDTO>>
    {
        public required Guid VolunteerGID { get; set; }
    }
}
