using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.ResourcesEventCommands.Update
{
    public class UpdateResourcesEventCommandHandler : IRequestHandler<UpdateResourcesEventCommand>
    {
        private readonly IRepository<ResourcesEvent> _resourcesEventRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<ResourcesEvent> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;
        public UpdateResourcesEventCommandHandler(
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

        public async Task Handle(UpdateResourcesEventCommand request, CancellationToken cancellationToken)
        {
            var entity = await _resourcesEventRepository.GetByGidAsync(request.ResourcesEvent.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(ResourcesEvent), request.ResourcesEvent.GID.ToString()));
            }

            if (await _eventRepository.GetByGidAsync(request.ResourcesEvent.EventGID, cancellationToken) == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.ResourcesEvent.EventGID.ToString()));
            }

            var resource = await _apiBuilder.GetRequest<ResourceDTO>($"utils/api/resource/{request.ResourcesEvent.ResourceGID}", Constants.UtilsService, cancellationToken, request.Token);
            if (resource == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, "Resource", request.ResourcesEvent.ResourceGID.ToString()));
            }

            var mappedEntity = _mapper.Map<ResourcesEvent>(request.ResourcesEvent);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

            await _resourcesEventRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
