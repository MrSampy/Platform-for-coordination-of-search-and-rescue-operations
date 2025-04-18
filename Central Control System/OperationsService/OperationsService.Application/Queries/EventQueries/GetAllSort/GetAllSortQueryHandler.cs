using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Responses;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.EventQueries.GetAllSort
{
    public class GetAllSortQueryHandler : IRequestHandler<GetAllSortQuery, GetAllEntitesReponse<EventDTO>>
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IMapper _mapper;

        public GetAllSortQueryHandler(IRepository<Event> eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<GetAllEntitesReponse<EventDTO>> Handle(GetAllSortQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            var result = await _eventRepository.GetAllAsync(cancellationToken);
            if (request.PaginationQuery.EventStatusGID != null)
            {
                result = result.Where(e => e.EventStatusGID == request.PaginationQuery.EventStatusGID);
            }

            var totalCount = result.Count();

            result = result.Skip((request.PaginationQuery.PageNumber - 1) * request.PaginationQuery.PageSize).Take(request.PaginationQuery.PageSize);

            return new GetAllEntitesReponse<EventDTO> { Items = _mapper.Map<IEnumerable<EventDTO>>(result), TotalCount = totalCount };
        }
    }
}
