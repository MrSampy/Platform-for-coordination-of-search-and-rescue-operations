using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.MessageCommands.Delete
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand>
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Message> _cacheService;

        public DeleteMessageCommandHandler(IRepository<Message> messageRepository, IUnitOfWork unitOfWork, ICacheService<Message> cacheService)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _messageRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Message), request.GID.ToString()));
            }

            await _messageRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
