using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.MeasurementUnitQueries.GetMeasurementUnitByGid
{
    public class GetMeasurementUnitByGidQueryHandler : IRequestHandler<GetMeasurementUnitByGidQuery, MeasurementUnit>
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;

        public GetMeasurementUnitByGidQueryHandler(IMeasurementUnitRepository measurementUnitRepository)
        {
            _measurementUnitRepository = measurementUnitRepository;
        }

        public async Task<MeasurementUnit> Handle(GetMeasurementUnitByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _measurementUnitRepository.GetByGidAsync(request.GID, cancellationToken);

            return result ?? throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(MeasurementUnit), request.GID.ToString()));
            ;
        }
    }
}
