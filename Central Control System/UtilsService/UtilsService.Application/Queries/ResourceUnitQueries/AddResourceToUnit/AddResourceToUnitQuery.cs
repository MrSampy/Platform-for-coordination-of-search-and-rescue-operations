using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourceUnitQueries.AddResourceToUnit
{
    public class AddResourceToUnitQuery : IRequest<ResourceMeasurementUnit>
    {
        public required ResourceMeasurementUnit ResourceMeasurementUnit { get; set; }
    }

}
