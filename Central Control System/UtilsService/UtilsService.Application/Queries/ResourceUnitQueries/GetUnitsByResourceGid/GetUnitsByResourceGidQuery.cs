using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourceUnitQueries.GetUnitsByResourceGid
{
    public class GetUnitsByResourceGidQuery : IRequest<IEnumerable<ResourceMeasurementUnit>>
    {
        public required Guid ResourceGID { get; set; }
    }
}
