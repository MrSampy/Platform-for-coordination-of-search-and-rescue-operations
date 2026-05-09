using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.MessageQueries.GetByGID
{
    public class GetMEssageByGidQueryHandler : IRequestHandler<GetMessageByGidQuery, MessageDTO>
    {
        private readonly IRepository<Message> _MessageRepository;
        private readonly IMapper _mapper;

        public GetMEssageByGidQueryHandler(IRepository<Message> MessageRepository, IMapper mapper)
        {
            _MessageRepository = MessageRepository;
            _mapper = mapper;
        }

        public async Task<MessageDTO> Handle(GetMessageByGidQuery request, CancellationToken cancellationToken)
        {
            var entity = await _MessageRepository.GetByGidAsync(request.GID, cancellationToken);
            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Message), request.GID));
            }
            return _mapper.Map<MessageDTO>(entity);
        }
    }
}
