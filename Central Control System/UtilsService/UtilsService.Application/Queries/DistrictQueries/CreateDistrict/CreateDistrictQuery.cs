using MediatR;
using UtilsService.Application.DTOs;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.DistrictQueries.CreateDistrict
{
    public class CreateDistrictQuery : IRequest<District>
    {
        public required CreateDistrictDTO District { get; set; }
    }
}
