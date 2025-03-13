using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetAll
{
    public class GetAllVolunteersDistrictsQueryHandler : IRequestHandler<GetAllVolunteersDistrictsQuery, IEnumerable<VolunteersDistrictsDTO>>
    {
        private readonly IRepository<VolunteersDistricts> _repository;
        private readonly ICacheService<VolunteersDistricts> _cacheService;
        private readonly IMapper _mapper;

        public GetAllVolunteersDistrictsQueryHandler(IRepository<VolunteersDistricts> repository, ICacheService<VolunteersDistricts> cacheService, IMapper mapper)
        {
            _repository = repository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteersDistrictsDTO>> Handle(GetAllVolunteersDistrictsQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new VolunteerServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllVolunteersDistrictsQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";
            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<VolunteersDistrictsDTO>>(cachedEntities);
            }

            var result = await _repository.GetAllAsync(cancellationToken, request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());
            return _mapper.Map<IEnumerable<VolunteersDistrictsDTO>>(result);
        }
    }
}
