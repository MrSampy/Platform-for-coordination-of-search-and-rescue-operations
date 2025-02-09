using MediatR;

namespace UtilsService.Application.Commands.MeasurementUnitCommands.DeleteMeasurementUnit
{
    public class DeleteMeasurementUnitCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
