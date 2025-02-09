using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.MeasurementUnitQueries.CreateMeasurementUnit
{
    public class CreateMeasurementUnitQuery : IRequest<MeasurementUnit>
    {
        public required MeasurementUnit MeasurementUnit { get; set; }
    }

}
