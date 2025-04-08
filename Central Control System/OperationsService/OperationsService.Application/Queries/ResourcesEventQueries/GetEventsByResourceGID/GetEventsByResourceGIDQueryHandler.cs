using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.Queries.ResourcesEventQueries.GetAll;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.ResourcesEventQueries.GetEventsByResourceGID
{
    public class GetEventsByResourceGIDQueryHandler : IRequestHandler<GetEventsByResourceGIDQuery, IEnumerable<ResourcesEventDTO>>
    {
        private readonly IRepository<ResourcesEvent> _resourcesEventRepository;
        private readonly ICacheService<ResourcesEvent> _cacheService;
        private readonly IMapper _mapper;

        public GetEventsByResourceGIDQueryHandler(
            IRepository<ResourcesEvent> resourcesEventRepository,
            ICacheService<ResourcesEvent> cacheService,
            IMapper mapper)
        {
            _resourcesEventRepository = resourcesEventRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResourcesEventDTO>> Handle(GetEventsByResourceGIDQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetEventsByResourceGIDQuery)}:{request.ResourceGID}";

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

            var result = getAllCachedEntities.Where(v => v.ResourceGID == request.ResourceGID);

            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<ResourcesEventDTO>>(result);
        }
    }
}
