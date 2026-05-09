using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.ResourcesQueries.GetAllResources
{
    public class GetAllResourcesQueryHandler : IRequestHandler<GetAllResourcesQuery, IEnumerable<Resource>>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ICacheService<Resource> _cacheService;

        public GetAllResourcesQueryHandler(IResourceRepository resourceRepository, ICacheService<Resource> cacheService)
        {
            _resourceRepository = resourceRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Resource>> Handle(GetAllResourcesQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new UtilsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllResourcesQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntites = _cacheService.Get(cacheKey);
            if (cachedEntites != null)
            {
                return cachedEntites;
            }

            var result = await _resourceRepository.GetAllAsync(cancellationToken, request.PaginationQuery.GetAll() ? null : request.PaginationQuery);

            _cacheService.Set(cacheKey, result.ToList());

            return result;
        }
    }
}
