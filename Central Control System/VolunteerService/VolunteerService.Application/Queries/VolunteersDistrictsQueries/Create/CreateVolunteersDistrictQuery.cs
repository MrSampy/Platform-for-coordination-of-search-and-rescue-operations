using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.DTOs.Create;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.Create
{
    public class CreateVolunteersDistrictQuery : IRequest<VolunteersDistrictsDTO>
    {
        public required CreateVolunteersDistrictsDTO VolunteersDistrictDTO { get; set; }
    }
}
