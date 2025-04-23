using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.EventCommands.Delete
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Event> _cacheService;

        public DeleteEventCommandHandler(IRepository<Event> eventRepository, IUnitOfWork unitOfWork, ICacheService<Event> cacheService)
        {
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var entity = await _eventRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.GID.ToString()));
            }

            entity.EventStatusGID = Constants.EventStatusDeleted;
            entity.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
