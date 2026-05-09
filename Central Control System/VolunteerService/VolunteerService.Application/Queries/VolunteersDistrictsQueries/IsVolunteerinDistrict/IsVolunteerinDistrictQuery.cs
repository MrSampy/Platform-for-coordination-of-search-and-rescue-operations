using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.DTOs.Create;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.IsVolunteerinDistrict
{
    public class IsVolunteerinDistrictQuery : IRequest<IsExistModel>
    {
        public required CreateVolunteersDistrictsDTO VolunteersDistrictsDTO { get; set; }
    }
}
