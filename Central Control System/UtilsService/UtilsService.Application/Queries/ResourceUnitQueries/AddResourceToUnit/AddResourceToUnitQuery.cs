using MediatR;
using UtilsService.Application.DTOs;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourceUnitQueries.AddResourceToUnit
{
    public class AddResourceToUnitQuery : IRequest<ResourceMeasurementUnit>
    {
        public required CreateResourceUnitDTO ResourceMeasurementUnit { get; set; }
    }

}
