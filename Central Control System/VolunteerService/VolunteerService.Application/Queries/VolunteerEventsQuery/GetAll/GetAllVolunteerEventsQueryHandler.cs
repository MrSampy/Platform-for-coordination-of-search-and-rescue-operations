using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.GetAll
{
    public class GetAllVolunteerEventsQueryHandler : IRequestHandler<GetAllVolunteerEventsQuery, IEnumerable<VolunteersEventsDTO>>
    {
        private readonly IRepository<VolunteersEvents> _volunteerEventRepository;
        private readonly ICacheService<VolunteersEvents> _cacheService;
        private readonly IMapper _mapper;

        public GetAllVolunteerEventsQueryHandler(IRepository<VolunteersEvents> volunteerEventRepository, ICacheService<VolunteersEvents> cacheService, IMapper mapper)
        {
            _volunteerEventRepository = volunteerEventRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteersEventsDTO>> Handle(GetAllVolunteerEventsQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new VolunteerServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllVolunteerEventsQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteersEventsDTO>>(cachedEntities);
            }

            var result = await _volunteerEventRepository.GetAllAsync(cancellationToken, request.PaginationQuery.GetAll() ? null : request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<VolunteersEventsDTO>>(result);
        }
    }
}
