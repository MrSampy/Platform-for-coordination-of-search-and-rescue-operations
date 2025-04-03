using AutoMapper;
using MediatR;
using VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetAll;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.IsVolunteerinDistrict
{
    public class IsVolunteerinDistrictQueryHandler : IRequestHandler<IsVolunteerinDistrictQuery, bool>
    {
        private readonly IRepository<VolunteersDistricts> _repository;
        private readonly ICacheService<VolunteersDistricts> _cacheService;
        private readonly IMapper _mapper;

        public IsVolunteerinDistrictQueryHandler(IRepository<VolunteersDistricts> repository, IMapper mapper, ICacheService<VolunteersDistricts> cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(IsVolunteerinDistrictQuery request, CancellationToken cancellationToken)
        {
            string getAllCacheKey = $"{nameof(GetAllVolunteersDistrictsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _repository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            return getAllCachedEntities.Any(vg => vg.VolunteerGID == request.VolunteersDistrictsDTO.VolunteerGID
                && vg.DistrictGID == request.VolunteersDistrictsDTO.DistrictGID);
        }
    }
}
