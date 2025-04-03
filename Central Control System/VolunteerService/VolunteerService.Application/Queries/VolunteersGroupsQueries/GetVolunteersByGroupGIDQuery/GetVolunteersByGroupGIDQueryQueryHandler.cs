using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetAll;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetGroupsByVolunteerGID;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.GetVolunteersByGroupGIDQuery
{
    public class GetVolunteersByGroupGIDQueryQueryHandler : IRequestHandler<GetVolunteersByGroupGIDQueryQuery, IEnumerable<VolunteersGroupsDTO>>
    {
        private readonly IRepository<VolunteersGroups> _volunteerGroupRepository;
        private readonly ICacheService<VolunteersGroups> _cacheService;
        private readonly IMapper _mapper;

        public GetVolunteersByGroupGIDQueryQueryHandler(IRepository<VolunteersGroups> volunteerGroupRepository, ICacheService<VolunteersGroups> cacheService, IMapper mapper)
        {
            _volunteerGroupRepository = volunteerGroupRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteersGroupsDTO>> Handle(GetVolunteersByGroupGIDQueryQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetGroupsByVolunteerGIDQuery)}:{request.GroupGID}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteersGroupsDTO>>(cachedEntities);
            }

            string getAllCacheKey = $"{nameof(GetAllVolunteerGroupsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _volunteerGroupRepository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            var result = getAllCachedEntities.Where(v => v.GroupGID == request.GroupGID);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<VolunteersGroupsDTO>>(result);
        }
    }
}
