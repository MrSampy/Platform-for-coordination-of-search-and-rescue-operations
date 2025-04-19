using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerQueries.GetAll
{
    public class GetAllVolunteersQueryHandler : IRequestHandler<GetAllVolunteersQuery, IEnumerable<VolunteerDTO>>
    {
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly ICacheService<Volunteer> _cacheService;
        private readonly IMapper _mapper;

        public GetAllVolunteersQueryHandler(IRepository<Volunteer> volunteerRepository, ICacheService<Volunteer> cacheService, IMapper mapper)
        {
            _volunteerRepository = volunteerRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteerDTO>> Handle(GetAllVolunteersQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new VolunteerServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllVolunteersQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteerDTO>>(cachedEntities);
            }

            var result = await _volunteerRepository.GetAllAsync(cancellationToken, request.PaginationQuery.GetAll() ? null : request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<VolunteerDTO>>(result);
        }
    }
}
