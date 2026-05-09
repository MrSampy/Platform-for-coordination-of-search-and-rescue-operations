using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.Queries.VolunteersGroupsQueries.GetAll;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.IsVolunteerinGroup
{
    public class IsVolunteerinGroupQueryHandler : IRequestHandler<IsVolunteerinGroupQuery, IsExistModel>
    {
        private readonly IRepository<VolunteersGroups> _repository;
        private readonly ICacheService<VolunteersGroups> _cacheService;
        private readonly IMapper _mapper;

        public IsVolunteerinGroupQueryHandler(IRepository<VolunteersGroups> repository, IMapper mapper, ICacheService<VolunteersGroups> cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<IsExistModel> Handle(IsVolunteerinGroupQuery request, CancellationToken cancellationToken)
        {
            string getAllCacheKey = $"{nameof(GetAllVolunteerGroupsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _repository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            return new IsExistModel()
            {
                IsExist = getAllCachedEntities.Any(vg => vg.VolunteerGID == request.VolunteersGroupsDTO.VolunteerGID
                && vg.GroupGID == request.VolunteersGroupsDTO.GroupGID)
            };
        }
    }
}
