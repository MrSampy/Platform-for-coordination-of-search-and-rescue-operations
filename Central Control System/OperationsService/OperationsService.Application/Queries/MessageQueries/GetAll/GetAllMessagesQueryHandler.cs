using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Responses;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.MessageQueries.GetAll
{
    public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, GetAllEntitesReponse<MessageDTO>>
    {
        private readonly IRepository<Message> _MessageRepository;
        private readonly IMapper _mapper;

        public GetAllMessagesQueryHandler(IRepository<Message> MessageRepository, IMapper mapper)
        {
            _MessageRepository = MessageRepository;
            _mapper = mapper;
        }

        public async Task<GetAllEntitesReponse<MessageDTO>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            var result = await _MessageRepository.GetAllAsync(cancellationToken);
            if (request.PaginationQuery.IsRead != null)
            {
                result = result.Where(e => e.IsRead == request.PaginationQuery.IsRead);
            }

            if (request.PaginationQuery.From != null)
            {
                result = result.Where(e => e.From == request.PaginationQuery.From);
            }

            if (request.PaginationQuery.To != null)
            {
                result = result.Where(e => e.To == request.PaginationQuery.To);
            }

            if (request.PaginationQuery.EventGID != null)
            {
                result = result.Where(e => e.EventGID == request.PaginationQuery.EventGID);
            }

            var totalCount = result.Count();

            if (!request.PaginationQuery.GetAll())
            {
                result = result.Skip((request.PaginationQuery.PageNumber - 1) * request.PaginationQuery.PageSize).Take(request.PaginationQuery.PageSize);
            }

            return new GetAllEntitesReponse<MessageDTO> { Items = _mapper.Map<IEnumerable<MessageDTO>>(result), TotalCount = totalCount };
        }
    }
}
