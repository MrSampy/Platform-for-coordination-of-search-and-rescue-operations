using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourceUnitQueries.IsResourceInUnit
{
    public class IsResourceInUnitQuery : IRequest<bool>
    {
        public required ResourceMeasurementUnit ResourceUnit { get; set; }
    }
}
