using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventTypeQueries.GetByGID
{
    public class GetEventTypeByGidQueryHandler : IRequestHandler<GetEventTypeByGidQuery, EventTypeDTO>
    {
        private readonly IRepository<EventType> _eventTypeRepository;
        private readonly IMapper _mapper;

        public GetEventTypeByGidQueryHandler(IRepository<EventType> eventTypeRepository, IMapper mapper)
        {
            _eventTypeRepository = eventTypeRepository;
            _mapper = mapper;
        }

        public async Task<EventTypeDTO> Handle(GetEventTypeByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventTypeRepository.GetByGidAsync(request.GID, cancellationToken);

            return result == null
                ? throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventType), request.GID.ToString()))
                : _mapper.Map<EventTypeDTO>(result);
        }
    }
}
