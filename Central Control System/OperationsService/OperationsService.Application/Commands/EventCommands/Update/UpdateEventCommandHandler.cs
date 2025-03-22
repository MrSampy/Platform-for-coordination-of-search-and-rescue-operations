using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Update;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.EventCommands.Update
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<EventType> _eventTypeRepository;
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly IRepository<EventStatus> _eventStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Event> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;

        public UpdateEventCommandHandler(
            IRepository<Event> eventRepository,
            IRepository<EventType> eventTypeRepository,
            IRepository<OperationWorker> operationWorkerRepository,
            IRepository<EventStatus> eventStatusRepository,
            IUnitOfWork unitOfWork,
            ICacheService<Event> cacheService,
            IMapper mapper,
            IApiBuilder apiBuilder)
        {
            _eventRepository = eventRepository;
            _eventTypeRepository = eventTypeRepository;
            _operationWorkerRepository = operationWorkerRepository;
            _eventStatusRepository = eventStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var entity = await _eventRepository.GetByGidAsync(request.Event.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.Event.GID.ToString()));
            }

            await ValidateRelatedEntities(request.Event, cancellationToken, request.Token);

            var mappedEntity = _mapper.Map<Event>(request.Event);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }

        private async Task ValidateRelatedEntities(UpdateEventDTO eventDto, CancellationToken cancellationToken, string token)
        {
            if (await _eventTypeRepository.GetByGidAsync(eventDto.EventTypeGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventType), eventDto.EventTypeGID.ToString()));

            if (await _operationWorkerRepository.GetByGidAsync(eventDto.CoordinatorGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), eventDto.CoordinatorGID.ToString()));

            if (await _operationWorkerRepository.GetByGidAsync(eventDto.DispatcherGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), eventDto.DispatcherGID.ToString()));

            if (await _eventStatusRepository.GetByGidAsync(eventDto.EventStatusGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventStatus), eventDto.EventStatusGID.ToString()));

            var district = await _apiBuilder.GetRequest<DistrictDTO>($"utils/api/district/{eventDto.DistrictGID}", Constants.UtilsService, cancellationToken, token);
            if (district == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, "District", eventDto.DistrictGID.ToString()));
            }
        }
    }

}
