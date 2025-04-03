using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetAll;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetDistrictsByVolunteerGID
{
    public class GetDistrictsByVolunteerGIDQueryHandler : IRequestHandler<GetDistrictsByVolunteerGIDQuery, IEnumerable<VolunteersDistrictsDTO>>
    {
        private readonly IRepository<VolunteersDistricts> _volunteerGroupRepository;
        private readonly ICacheService<VolunteersDistricts> _cacheService;
        private readonly IMapper _mapper;

        public GetDistrictsByVolunteerGIDQueryHandler(IRepository<VolunteersDistricts> volunteerGroupRepository, ICacheService<VolunteersDistricts> cacheService, IMapper mapper)
        {
            _volunteerGroupRepository = volunteerGroupRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteersDistrictsDTO>> Handle(GetDistrictsByVolunteerGIDQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetDistrictsByVolunteerGIDQuery)}:{request.VolunteerGID}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteersDistrictsDTO>>(cachedEntities);
            }

            string getAllCacheKey = $"{nameof(GetAllVolunteersDistrictsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _volunteerGroupRepository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            var result = getAllCachedEntities.Where(v => v.VolunteerGID == request.VolunteerGID);

            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<VolunteersDistrictsDTO>>(result);
        }
    }
}
