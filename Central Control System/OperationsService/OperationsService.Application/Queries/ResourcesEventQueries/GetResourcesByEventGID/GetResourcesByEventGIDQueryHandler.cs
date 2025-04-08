using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.Queries.ResourcesEventQueries.GetAll;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.ResourcesEventQueries.GetResourcesByEventGID
{
    public class GetResourcesByEventGIDQueryHandler : IRequestHandler<GetResourcesByEventGIDQuery, IEnumerable<ResourcesEventDTO>>
    {
        private readonly IRepository<ResourcesEvent> _resourcesEventRepository;
        private readonly ICacheService<ResourcesEvent> _cacheService;
        private readonly IMapper _mapper;

        public GetResourcesByEventGIDQueryHandler(
            IRepository<ResourcesEvent> resourcesEventRepository,
            ICacheService<ResourcesEvent> cacheService,
            IMapper mapper)
        {
            _resourcesEventRepository = resourcesEventRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResourcesEventDTO>> Handle(GetResourcesByEventGIDQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetResourcesByEventGIDQuery)}:{request.EventGID}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<ResourcesEventDTO>>(cachedEntities);
            }

            string getAllCacheKey = $"{nameof(GetAllResourcesEventsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _resourcesEventRepository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            var result = getAllCachedEntities.Where(v => v.EventGID == request.EventGID);

            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<ResourcesEventDTO>>(result);
        }
    }
}
