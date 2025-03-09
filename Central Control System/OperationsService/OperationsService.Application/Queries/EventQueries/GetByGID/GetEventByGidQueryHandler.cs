using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventQueries.GetByGID
{
    public class GetEventByGidQueryHandler : IRequestHandler<GetEventByGidQuery, EventDTO>
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IMapper _mapper;

        public GetEventByGidQueryHandler(IRepository<Event> eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<EventDTO> Handle(GetEventByGidQuery request, CancellationToken cancellationToken)
        {
            var entity = await _eventRepository.GetByGidAsync(request.GID, cancellationToken);
            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.GID));
            }
            return _mapper.Map<EventDTO>(entity);
        }
    }

}
