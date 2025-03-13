using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.GetAll
{
    public class GetAllVolunteerGroupsQueryHandler : IRequestHandler<GetAllVolunteerGroupsQuery, IEnumerable<VolunteersGroupsDTO>>
    {
        private readonly IRepository<VolunteersGroups> _volunteerGroupRepository;
        private readonly ICacheService<VolunteersGroups> _cacheService;
        private readonly IMapper _mapper;

        public GetAllVolunteerGroupsQueryHandler(IRepository<VolunteersGroups> volunteerGroupRepository, ICacheService<VolunteersGroups> cacheService, IMapper mapper)
        {
            _volunteerGroupRepository = volunteerGroupRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteersGroupsDTO>> Handle(GetAllVolunteerGroupsQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new VolunteerServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllVolunteerGroupsQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteersGroupsDTO>>(cachedEntities);
            }

            var result = await _volunteerGroupRepository.GetAllAsync(cancellationToken, request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<VolunteersGroupsDTO>>(result);
        }
    }
}
