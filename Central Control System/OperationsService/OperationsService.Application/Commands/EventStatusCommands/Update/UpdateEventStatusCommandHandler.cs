using AutoMapper;
using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.EventStatusCommands.Update
{
    public class UpdateEventStatusCommandHandler : IRequestHandler<UpdateEventStatusCommand>
    {
        private readonly IRepository<EventStatus> _eventStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<EventStatus> _cacheService;
        private readonly IMapper _mapper;

        public UpdateEventStatusCommandHandler(IRepository<EventStatus> eventStatusRepository, IUnitOfWork unitOfWork, ICacheService<EventStatus> cacheService, IMapper mapper)
        {
            _eventStatusRepository = eventStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task Handle(UpdateEventStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _eventStatusRepository.GetByGidAsync(request.EventStatus.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventStatus), request.EventStatus.GID.ToString()));
            }
            var mappedEntity = _mapper.Map<EventStatus>(request.EventStatus);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.Now;

            await _eventStatusRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
