using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.EventTypeCommands.Delete
{
    public class DeleteEventTypeCommandHandler : IRequestHandler<DeleteEventTypeCommand>
    {
        private readonly IRepository<EventType> _eventTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<EventType> _cacheService;

        public DeleteEventTypeCommandHandler(IRepository<EventType> eventTypeRepository, IUnitOfWork unitOfWork, ICacheService<EventType> cacheService)
        {
            _eventTypeRepository = eventTypeRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteEventTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _eventTypeRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventType), request.GID.ToString()));
            }

            await _eventTypeRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
