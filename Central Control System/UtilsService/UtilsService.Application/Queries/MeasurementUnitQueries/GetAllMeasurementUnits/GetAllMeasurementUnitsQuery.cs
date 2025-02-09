using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.MeasurementUnitQueries.GetAllMeasurementUnits
{
    public class GetAllMeasurementUnitsQuery : IRequest<IEnumerable<MeasurementUnit>>
    {
        public PaginationQuery PaginationQuery { get; set; }
    }
}
