using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventTypeQueries.Create
{
    public class CreateEventTypeQueryHandler : IRequestHandler<CreateEventTypeQuery, EventTypeDTO>
    {
        private readonly IRepository<EventType> _eventTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<EventType> _cacheService;
        private readonly IMapper _mapper;

        public CreateEventTypeQueryHandler(IRepository<EventType> eventTypeRepository, IUnitOfWork unitOfWork, ICacheService<EventType> cacheService, IMapper mapper)
        {
            _eventTypeRepository = eventTypeRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<EventTypeDTO> Handle(CreateEventTypeQuery request, CancellationToken cancellationToken)
        {
            var eventType = _mapper.Map<EventType>(request.EventType);
            eventType.GID = Guid.NewGuid();
            eventType.CreatedAt = eventType.UpdatedAt = DateTime.Now;
            await _eventTypeRepository.AddAsync(eventType);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<EventTypeDTO>(eventType);
        }
    }

}
