using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetAll;
using VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetDistrictsByVolunteerGID;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetVolunteersByDistrictGIDQuery
{
    public class GetVolunteersByDistrictGIDQueryQueryHandler : IRequestHandler<GetVolunteersByDistrictGIDQueryQuery, IEnumerable<VolunteersDistrictsDTO>>
    {
        private readonly IRepository<VolunteersDistricts> _volunteerDistrictRepository;
        private readonly ICacheService<VolunteersDistricts> _cacheService;
        private readonly IMapper _mapper;

        public GetVolunteersByDistrictGIDQueryQueryHandler(IRepository<VolunteersDistricts> volunteerDistrictRepository, ICacheService<VolunteersDistricts> cacheService, IMapper mapper)
        {
            _volunteerDistrictRepository = volunteerDistrictRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteersDistrictsDTO>> Handle(GetVolunteersByDistrictGIDQueryQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetDistrictsByVolunteerGIDQuery)}:{request.DistrictGID}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteersDistrictsDTO>>(cachedEntities);
            }

            string getAllCacheKey = $"{nameof(GetAllVolunteersDistrictsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _volunteerDistrictRepository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            var result = getAllCachedEntities.Where(v => v.DistrictGID == request.DistrictGID);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<VolunteersDistrictsDTO>>(result);
        }
    }
}
