using MediatR;
using VolunteerService.Application.DTOs.Create;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.IsVolunteerinDistrict
{
    public class IsVolunteerinDistrictQuery : IRequest<bool>
    {
        public required CreateVolunteersDistrictsDTO VolunteersDistrictsDTO { get; set; }
    }
}
