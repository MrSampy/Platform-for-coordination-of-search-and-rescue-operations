using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventTypeQueries.GetAll
{
    public class GetAllEventTypesQueryHandler : IRequestHandler<GetAllEventTypesQuery, IEnumerable<EventTypeDTO>>
    {
        private readonly IRepository<EventType> _eventTypeRepository;
        private readonly ICacheService<EventType> _cacheService;
        private readonly IMapper _mapper;

        public GetAllEventTypesQueryHandler(IRepository<EventType> eventTypeRepository, ICacheService<EventType> cacheService, IMapper mapper)
        {
            _eventTypeRepository = eventTypeRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventTypeDTO>> Handle(GetAllEventTypesQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllEventTypesQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<EventTypeDTO>>(cachedEntities);
            }

            var result = await _eventTypeRepository.GetAllAsync(cancellationToken, request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<EventTypeDTO>>(result);
        }
    }
}
