using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.EventStatusCommands.Delete
{
    public class DeleteEventStatusCommandHandler : IRequestHandler<DeleteEventStatusCommand>
    {
        private readonly IRepository<EventStatus> _eventStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<EventStatus> _cacheService;

        public DeleteEventStatusCommandHandler(IRepository<EventStatus> eventStatusRepository, IUnitOfWork unitOfWork, ICacheService<EventStatus> cacheService)
        {
            _eventStatusRepository = eventStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteEventStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _eventStatusRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventStatus), request.GID.ToString()));
            }

            await _eventStatusRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
