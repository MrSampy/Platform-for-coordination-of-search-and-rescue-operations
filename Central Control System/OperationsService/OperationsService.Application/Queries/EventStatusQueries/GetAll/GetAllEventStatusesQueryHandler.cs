using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventStatusQueries.GetAll
{
    public class GetAllEventStatusesQueryHandler : IRequestHandler<GetAllEventStatusesQuery, IEnumerable<EventStatusDTO>>
    {
        private readonly IRepository<EventStatus> _eventStatusRepository;
        private readonly ICacheService<EventStatus> _cacheService;
        private readonly IMapper _mapper;

        public GetAllEventStatusesQueryHandler(IRepository<EventStatus> eventStatusRepository, ICacheService<EventStatus> cacheService, IMapper mapper)
        {
            _eventStatusRepository = eventStatusRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventStatusDTO>> Handle(GetAllEventStatusesQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllEventStatusesQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<EventStatusDTO>>(cachedEntities);
            }

            var result = await _eventStatusRepository.GetAllAsync(cancellationToken, request.PaginationQuery.GetAll() ? null : request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<EventStatusDTO>>(result);
        }
    }
}
