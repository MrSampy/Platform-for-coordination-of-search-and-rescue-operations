using MediatR;
using UtilsService.Application.DTOs;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourceUnitQueries.IsResourceInUnit
{
    public class IsResourceInUnitQuery : IRequest<IsExistModel>
    {
        public required ResourceMeasurementUnit ResourceUnit { get; set; }
    }
}
