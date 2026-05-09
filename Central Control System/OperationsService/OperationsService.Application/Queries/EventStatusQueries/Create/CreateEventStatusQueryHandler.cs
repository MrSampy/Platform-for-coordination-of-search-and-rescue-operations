using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventStatusQueries.Create
{
    public class CreateEventStatusQueryHandler : IRequestHandler<CreateEventStatusQuery, EventStatusDTO>
    {
        private readonly IRepository<EventStatus> _eventStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<EventStatus> _cacheService;
        private readonly IMapper _mapper;

        public CreateEventStatusQueryHandler(IRepository<EventStatus> eventStatusRepository, IUnitOfWork unitOfWork, ICacheService<EventStatus> cacheService, IMapper mapper)
        {
            _eventStatusRepository = eventStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<EventStatusDTO> Handle(CreateEventStatusQuery request, CancellationToken cancellationToken)
        {
            var eventStatus = _mapper.Map<EventStatus>(request.EventStatus);
            eventStatus.GID = Guid.NewGuid();
            eventStatus.CreatedAt = eventStatus.UpdatedAt = DateTime.UtcNow;
            await _eventStatusRepository.AddAsync(eventStatus);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<EventStatusDTO>(eventStatus);
        }
    }

}
