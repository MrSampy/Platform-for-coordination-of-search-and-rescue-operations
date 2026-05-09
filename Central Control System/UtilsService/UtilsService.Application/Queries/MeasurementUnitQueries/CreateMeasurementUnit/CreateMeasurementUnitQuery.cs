using MediatR;
using UtilsService.Application.DTOs;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.MeasurementUnitQueries.CreateMeasurementUnit
{
    public class CreateMeasurementUnitQuery : IRequest<MeasurementUnit>
    {
        public required CreateMeasurementUnitDTO MeasurementUnit { get; set; }
    }

}
