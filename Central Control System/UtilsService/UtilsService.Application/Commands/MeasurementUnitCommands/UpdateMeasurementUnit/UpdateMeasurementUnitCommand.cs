using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Commands.MeasurementUnitCommands.UpdateMeasurementUnit
{
    public class UpdateMeasurementUnitCommand : IRequest
    {
        public required MeasurementUnit MeasurementUnit { get; set; }
    }

}
