using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourceUnitQueries.GetResourcesByUnitGid
{
    public class GetResourcesByUnitGidQuery : IRequest<IEnumerable<ResourceMeasurementUnit>>
    {
        public required Guid UnitGID { get; set; }
    }
}
