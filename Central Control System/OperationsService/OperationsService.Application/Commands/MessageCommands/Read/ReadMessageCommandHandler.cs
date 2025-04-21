using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.MessageCommands.Read
{
    public class ReadMessageCommandHandler : IRequestHandler<ReadMessageCommand>
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Message> _cacheService;

        public ReadMessageCommandHandler(IRepository<Message> messageRepository, IUnitOfWork unitOfWork, ICacheService<Message> cacheService)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(ReadMessageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _messageRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Message), request.GID.ToString()));
            }

            entity.IsRead = true;

            await _messageRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
