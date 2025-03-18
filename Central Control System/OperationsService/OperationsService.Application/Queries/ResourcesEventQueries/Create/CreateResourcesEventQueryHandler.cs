using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.ResourcesEventQueries.Create
{
    public class CreateResourcesEventQueryHandler : IRequestHandler<CreateResourcesEventQuery, ResourcesEventDTO>
    {
        private readonly IRepository<ResourcesEvent> _resourcesEventRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<ResourcesEvent> _cacheService;
        private readonly IMapper _mapper;

        public CreateResourcesEventQueryHandler(
            IRepository<ResourcesEvent> resourcesEventRepository,
            IRepository<Event> eventRepository,
            IUnitOfWork unitOfWork,
            ICacheService<ResourcesEvent> cacheService,
            IMapper mapper)
        {
            _resourcesEventRepository = resourcesEventRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<ResourcesEventDTO> Handle(CreateResourcesEventQuery request, CancellationToken cancellationToken)
        {
            if (await _eventRepository.GetByGidAsync(request.ResourcesEvent.EventGID, cancellationToken) == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.ResourcesEvent.EventGID.ToString()));
            }

            var resourcesEvent = _mapper.Map<ResourcesEvent>(request.ResourcesEvent);
            resourcesEvent.GID = Guid.NewGuid();
            resourcesEvent.CreatedAt = resourcesEvent.UpdatedAt = DateTime.UtcNow;
            await _resourcesEventRepository.AddAsync(resourcesEvent);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<ResourcesEventDTO>(resourcesEvent);
        }
    }

}
