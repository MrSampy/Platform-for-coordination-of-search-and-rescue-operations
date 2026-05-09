using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.MeasurementUnitQueries.GetMeasurementUnitByGid
{
    public class GetMeasurementUnitByGidQuery : IRequest<MeasurementUnit>
    {
        public required Guid GID { get; set; }
    }
}
