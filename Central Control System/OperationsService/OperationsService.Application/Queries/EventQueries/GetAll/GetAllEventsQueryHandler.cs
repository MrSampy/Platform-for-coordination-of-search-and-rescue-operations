using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventQueries.GetAll
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, IEnumerable<EventDTO>>
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly ICacheService<Event> _cacheService;
        private readonly IMapper _mapper;

        public GetAllEventsQueryHandler(IRepository<Event> eventRepository, ICacheService<Event> cacheService, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDTO>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllEventsQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<EventDTO>>(cachedEntities);
            }

            var result = await _eventRepository.GetAllAsync(cancellationToken, request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<EventDTO>>(result);
        }
    }
}
