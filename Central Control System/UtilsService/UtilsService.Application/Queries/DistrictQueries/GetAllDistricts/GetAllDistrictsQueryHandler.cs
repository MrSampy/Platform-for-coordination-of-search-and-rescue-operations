using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.DistrictQueries.GetAllDistricts
{
    public class GetAllDistrictsQueryHandler : IRequestHandler<GetAllDistrictsQuery, IEnumerable<District>>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly ICacheService<District> _cacheService;

        public GetAllDistrictsQueryHandler(IDistrictRepository districtRepository, ICacheService<District> cacheService)
        {
            _districtRepository = districtRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<District>> Handle(GetAllDistrictsQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new CustomException(Constants.InvalidPagonationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllDistrictsQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntites = _cacheService.Get(cacheKey);
            if (cachedEntites != null)
            {
                return cachedEntites;
            }

            var result = await _districtRepository.GetAllAsync(cancellationToken, request.PaginationQuery);

            _cacheService.Set(cacheKey, result.ToList());

            return result;
        }
    }
}
