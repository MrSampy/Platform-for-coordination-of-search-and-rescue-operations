using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventQueries.Create
{
    public class CreateEventQueryHandler : IRequestHandler<CreateEventQuery, EventDTO>
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<EventType> _eventTypeRepository;
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly IRepository<EventStatus> _eventStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Event> _cacheService;
        private readonly IMapper _mapper;

        public CreateEventQueryHandler(
            IRepository<Event> eventRepository,
            IRepository<EventType> eventTypeRepository,
            IRepository<OperationWorker> operationWorkerRepository,
            IRepository<EventStatus> eventStatusRepository,
            IUnitOfWork unitOfWork,
            ICacheService<Event> cacheService,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _eventTypeRepository = eventTypeRepository;
            _operationWorkerRepository = operationWorkerRepository;
            _eventStatusRepository = eventStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<EventDTO> Handle(CreateEventQuery request, CancellationToken cancellationToken)
        {
            await ValidateRelatedEntities(request.Event, cancellationToken);

            var newEvent = _mapper.Map<Event>(request.Event);
            newEvent.GID = Guid.NewGuid();

            newEvent.CreatedAt = newEvent.UpdatedAt = DateTime.Now;

            await _eventRepository.AddAsync(newEvent);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<EventDTO>(newEvent);
        }

        private async Task ValidateRelatedEntities(CreateEventDTO eventDto, CancellationToken cancellationToken)
        {
            if (await _eventTypeRepository.GetByGidAsync(eventDto.EventTypeGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventType), eventDto.EventTypeGID.ToString()));

            if (await _operationWorkerRepository.GetByGidAsync(eventDto.CoordinatorGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), eventDto.CoordinatorGID.ToString()));

            if (await _operationWorkerRepository.GetByGidAsync(eventDto.DispatcherGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), eventDto.DispatcherGID.ToString()));

            if (await _eventStatusRepository.GetByGidAsync(eventDto.EventStatusGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventStatus), eventDto.EventStatusGID.ToString()));
        }
    }

}
