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
        private readonly IApiBuilder _apiBuilder;
        public CreateResourcesEventQueryHandler(
            IRepository<ResourcesEvent> resourcesEventRepository,
            IRepository<Event> eventRepository,
            IUnitOfWork unitOfWork,
            ICacheService<ResourcesEvent> cacheService,
            IMapper mapper,
            IApiBuilder apiBuilder)
        {
            _resourcesEventRepository = resourcesEventRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task<ResourcesEventDTO> Handle(CreateResourcesEventQuery request, CancellationToken cancellationToken)
        {
            if (await _eventRepository.GetByGidAsync(request.ResourcesEvent.EventGID, cancellationToken) == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.ResourcesEvent.EventGID.ToString()));
            }

            var resource = await _apiBuilder.GetRequest<ResourceDTO>($"utils/api/resource/{request.ResourcesEvent.ResourceGID}", Constants.UtilsService, cancellationToken, request.Token);
            if (resource == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, "Resource", request.ResourcesEvent.ResourceGID.ToString()));
            }

            var measurementUnit = await _apiBuilder.GetRequest<MeasurementUnitDTO>($"utils/api/measurementUnit/{request.ResourcesEvent.MeasurementUnitGID}", Constants.UtilsService, cancellationToken, request.Token);
            if (measurementUnit == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, "MeasurementUnit", request.ResourcesEvent.MeasurementUnitGID.ToString()));
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
