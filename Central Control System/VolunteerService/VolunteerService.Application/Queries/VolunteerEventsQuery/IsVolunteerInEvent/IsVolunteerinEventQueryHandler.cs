using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.Queries.VolunteerEventsQuery.GetAll;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.IsVolunteerInEvent
{
    public class IsVolunteerinEventQueryHandler : IRequestHandler<IsVolunteerinEventQuery, IsExistModel>
    {
        private readonly IRepository<VolunteersEvents> _repository;
        private readonly ICacheService<VolunteersEvents> _cacheService;
        private readonly IMapper _mapper;

        public IsVolunteerinEventQueryHandler(IRepository<VolunteersEvents> repository, IMapper mapper, ICacheService<VolunteersEvents> cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<IsExistModel> Handle(IsVolunteerinEventQuery request, CancellationToken cancellationToken)
        {
            string getAllCacheKey = $"{nameof(GetAllVolunteerEventsQuery)}";

            var getAllCachedEntities = _cacheService.Get(getAllCacheKey);

            if (getAllCachedEntities == null)
            {
                getAllCachedEntities = (await _repository.GetAllAsync(cancellationToken)).ToList();
                _cacheService.Set(getAllCacheKey, getAllCachedEntities);
            }

            return new IsExistModel()
            {
                IsExist = getAllCachedEntities.Any(vg => vg.VolunteerGID == request.VolunteersEventsDTO.VolunteerGID
                && vg.EventGID == request.VolunteersEventsDTO.EventGID)
            };
        }
    }
}
