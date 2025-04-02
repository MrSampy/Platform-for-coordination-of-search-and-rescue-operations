using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.GetGroupsByVolunteerGID
{
    public class GetGroupsByVolunteerGIDQueryHandler : IRequestHandler<GetGroupsByVolunteerGIDQuery, IEnumerable<VolunteersGroupsDTO>>
    {
        private readonly IRepository<VolunteersGroups> _volunteerGroupRepository;
        private readonly ICacheService<VolunteersGroups> _cacheService;
        private readonly IMapper _mapper;

        public GetGroupsByVolunteerGIDQueryHandler(IRepository<VolunteersGroups> volunteerGroupRepository, ICacheService<VolunteersGroups> cacheService, IMapper mapper)
        {
            _volunteerGroupRepository = volunteerGroupRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteersGroupsDTO>> Handle(GetGroupsByVolunteerGIDQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{nameof(GetGroupsByVolunteerGIDQuery)}:{request.VolunteerGID}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteersGroupsDTO>>(cachedEntities);
            }

            var result = (await _volunteerGroupRepository.GetAllAsync(cancellationToken)).Where(v => v.VolunteerGID == request.VolunteerGID);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<VolunteersGroupsDTO>>(result);
        }
    }
}
