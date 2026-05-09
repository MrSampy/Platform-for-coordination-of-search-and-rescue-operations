using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.MessageQueries.Create
{
    public class CreateMessageQueryHandler : IRequestHandler<CreateMessageQuery, MessageDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Message> _cacheService;
        private readonly IMapper _mapper;

        public CreateMessageQueryHandler(IUnitOfWork unitOfWork, ICacheService<Message> cacheService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<MessageDTO> Handle(CreateMessageQuery request, CancellationToken cancellationToken)
        {
            await ValidateRelatedEntities(request.Message, cancellationToken);

            var message = _mapper.Map<Message>(request.Message);
            message.GID = Guid.NewGuid();
            message.CreatedAt = message.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<MessageDTO>(message);
        }
        private async Task ValidateRelatedEntities(CreateMessageDTO messageDTO, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.OperationWorkerRepository.GetByGidAsync(messageDTO.From, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), messageDTO.From.ToString()));

            if (await _unitOfWork.OperationWorkerRepository.GetByGidAsync(messageDTO.To, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), messageDTO.To.ToString()));

            if (await _unitOfWork.EventRepository.GetByGidAsync(messageDTO.EventGID, cancellationToken) == null)
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), messageDTO.EventGID.ToString()));
        }
    }
}
