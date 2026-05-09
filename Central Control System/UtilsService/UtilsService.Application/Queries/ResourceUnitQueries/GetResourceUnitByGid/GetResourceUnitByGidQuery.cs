using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourceUnitQueries.GetResourceUnitByGid
{
    public class GetResourceUnitByGidQuery : IRequest<ResourceMeasurementUnit>
    {
        public required Guid GID { get; set; }
    }
}
