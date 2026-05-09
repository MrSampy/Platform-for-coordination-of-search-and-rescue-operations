using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.DistrictQueries.GetAllDistricts
{
    public class GetAllDistrictsQuery : IRequest<IEnumerable<District>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
