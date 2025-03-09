using AutoMapper;
using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.EventTypeCommands.Update
{
    public class UpdateEventTypeCommandHandler : IRequestHandler<UpdateEventTypeCommand>
    {
        private readonly IRepository<EventType> _eventTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<EventType> _cacheService;
        private readonly IMapper _mapper;

        public UpdateEventTypeCommandHandler(IRepository<EventType> eventTypeRepository, IUnitOfWork unitOfWork, ICacheService<EventType> cacheService, IMapper mapper)
        {
            _eventTypeRepository = eventTypeRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task Handle(UpdateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _eventTypeRepository.GetByGidAsync(request.EventType.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventType), request.EventType.GID.ToString()));
            }

            var mappedEntity = _mapper.Map<EventType>(request.EventType);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.Now;

            await _eventTypeRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
