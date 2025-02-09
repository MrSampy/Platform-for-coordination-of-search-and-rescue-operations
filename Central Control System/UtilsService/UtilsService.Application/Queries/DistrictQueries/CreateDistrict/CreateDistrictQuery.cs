using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.DistrictQueries.CreateDistrict
{
    public class CreateDistrictQuery : IRequest<District>
    {
        public required District District { get; set; }
    }
}
