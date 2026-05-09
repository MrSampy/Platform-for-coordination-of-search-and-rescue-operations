using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.ResourceUnitQueries.GetUnitsByResourceGid
{
    public class GetUnitsByResourceGidQueryHandler : IRequestHandler<GetUnitsByResourceGidQuery, IEnumerable<ResourceMeasurementUnit>>
    {
        private readonly IResourceMeasurementUnitRepository _repository;
        private readonly ICacheService<ResourceMeasurementUnit> _cacheService;
        public GetUnitsByResourceGidQueryHandler(IResourceMeasurementUnitRepository repository, ICacheService<ResourceMeasurementUnit> cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<ResourceMeasurementUnit>> Handle(GetUnitsByResourceGidQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetUnitsByResourceGidQueryHandler)}:{request.ResourceGID}";

            var cachedEntites = _cacheService.Get(cacheKey);
            if (cachedEntites != null)
            {
                return cachedEntites;
            }

            var result = await _repository.GetMeasurementUnitsByResorceGIDAsync(request.ResourceGID, cancellationToken);

            _cacheService.Set(cacheKey, result.ToList());

            return result;
        }
    }
}
