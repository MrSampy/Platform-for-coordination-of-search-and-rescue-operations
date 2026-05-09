using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.DistrictQueries.GetDistrictByGid
{
    public class GetDistrictByGidQuery : IRequest<District>
    {
        public required Guid GID { get; set; }
    }
}
