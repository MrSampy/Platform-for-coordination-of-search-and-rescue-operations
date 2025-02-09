using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.MeasurementUnitQueries.GetAllMeasurementUnits
{
    public class GetAllMeasurementUnitsQueryHandler : IRequestHandler<GetAllMeasurementUnitsQuery, IEnumerable<MeasurementUnit>>
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;
        private readonly ICacheService<MeasurementUnit> _cacheService;

        public GetAllMeasurementUnitsQueryHandler(IMeasurementUnitRepository measurementUnitRepository, ICacheService<MeasurementUnit> cacheService)
        {
            _measurementUnitRepository = measurementUnitRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<MeasurementUnit>> Handle(GetAllMeasurementUnitsQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new CustomException(Constants.InvalidPagonationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllMeasurementUnitsQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntites = _cacheService.Get(cacheKey);
            if (cachedEntites != null)
            {
                return cachedEntites;
            }

            var result = await _measurementUnitRepository.GetAllAsync(cancellationToken, request.PaginationQuery);

            _cacheService.Set(cacheKey, result.ToList());

            return result;
        }
    }

}
