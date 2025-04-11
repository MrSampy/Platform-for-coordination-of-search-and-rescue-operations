using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.Queries.EventQueries.GetAll;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventQueries.GetByStatusGID
{
    public class GetByStatusGIDQueryHandler : IRequestHandler<GetByStatusGIDQuery, IEnumerable<EventDTO>>
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly ICacheService<Event> _cacheService;
        private readonly IMapper _mapper;

        public GetByStatusGIDQueryHandler(IRepository<Event> eventRepository, ICacheService<Event> cacheService, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }
        public async Task<IEnumerable<EventDTO>> Handle(GetByStatusGIDQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetByStatusGIDQuery)}:{request.EventStatusGID}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<EventDTO>>(cachedEntities);
            }

            string getAllCacheKey = $"{nameof(GetAllEventsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _eventRepository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            var result = getAllCachedEntities.Where(v => v.EventStatusGID == request.EventStatusGID);

            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<EventDTO>>(result);
        }
    }
}
