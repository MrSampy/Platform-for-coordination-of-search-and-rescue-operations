using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.ResourceUnitQueries.GetResourcesByUnitGid
{
    public class GetResourcesByUnitGidQueryHandler : IRequestHandler<GetResourcesByUnitGidQuery, IEnumerable<ResourceMeasurementUnit>>
    {
        private readonly IResourceMeasurementUnitRepository _repository;
        private readonly ICacheService<ResourceMeasurementUnit> _cacheService;

        public GetResourcesByUnitGidQueryHandler(IResourceMeasurementUnitRepository repository, ICacheService<ResourceMeasurementUnit> cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<ResourceMeasurementUnit>> Handle(GetResourcesByUnitGidQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetResourcesByUnitGidQueryHandler)}:{request.UnitGID}";

            var cachedEntites = _cacheService.Get(cacheKey);
            if (cachedEntites != null)
            {
                return cachedEntites;
            }

            var result = await _repository.GetResourcesByMeasurementUnitGIDAsync(request.UnitGID, cancellationToken);

            _cacheService.Set(cacheKey, result.ToList());

            return result;
        }
    }
}
