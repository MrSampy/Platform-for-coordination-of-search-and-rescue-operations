using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventStatusQueries.GetByGID
{
    public class GetEventStatusByGidQueryHandler : IRequestHandler<GetEventStatusByGidQuery, EventStatusDTO>
    {
        private readonly IRepository<EventStatus> _eventStatusRepository;
        private readonly IMapper _mapper;

        public GetEventStatusByGidQueryHandler(IRepository<EventStatus> eventStatusRepository, IMapper mapper)
        {
            _eventStatusRepository = eventStatusRepository;
            _mapper = mapper;
        }

        public async Task<EventStatusDTO> Handle(GetEventStatusByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventStatusRepository.GetByGidAsync(request.GID, cancellationToken);

            return result == null
                ? throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(EventStatus), request.GID.ToString()))
                : _mapper.Map<EventStatusDTO>(result);
        }
    }
}
