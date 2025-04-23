using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.Queries.VolunteerEventsQuery.GetAll;
using VolunteerService.Application.Queries.VolunteerEventsQuery.GetEventsByVolunteerGID;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.GetVolunteersByEventGIDQuery
{
    public class GetVolunteersByEventGIDQueryQueryHandler : IRequestHandler<GetVolunteersByEventGIDQueryQuery, IEnumerable<VolunteersEventsDTO>>
    {
        private readonly IRepository<VolunteersEvents> _volunteerEventRepository;
        private readonly ICacheService<VolunteersEvents> _cacheService;
        private readonly IMapper _mapper;

        public GetVolunteersByEventGIDQueryQueryHandler(IRepository<VolunteersEvents> volunteerEventRepository, ICacheService<VolunteersEvents> cacheService, IMapper mapper)
        {
            _volunteerEventRepository = volunteerEventRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteersEventsDTO>> Handle(GetVolunteersByEventGIDQueryQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetEventsByVolunteerGIDQuery)}:{request.EventGID}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteersEventsDTO>>(cachedEntities);
            }

            string getAllCacheKey = $"{nameof(GetAllVolunteerEventsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _volunteerEventRepository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            var result = getAllCachedEntities.Where(v => v.EventGID == request.EventGID);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<VolunteersEventsDTO>>(result);
        }
    }
}
