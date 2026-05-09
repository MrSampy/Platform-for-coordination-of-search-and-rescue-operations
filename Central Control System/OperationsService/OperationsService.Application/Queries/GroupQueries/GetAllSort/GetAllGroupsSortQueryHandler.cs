using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Responses;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.GroupQueries.GetAllSort
{
    public class GetAllGroupsSortQueryHandler : IRequestHandler<GetAllGroupsSortQuery, GetAllEntitesReponse<GroupDTO>>
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IMapper _mapper;

        public GetAllGroupsSortQueryHandler(IRepository<Group> GroupRepository, IMapper mapper)
        {
            _groupRepository = GroupRepository;
            _mapper = mapper;
        }

        public async Task<GetAllEntitesReponse<GroupDTO>> Handle(GetAllGroupsSortQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            var result = await _groupRepository.GetAllAsync(cancellationToken);
            if (request.PaginationQuery.EventGID != null)
            {
                result = result.Where(e => e.EventGID == request.PaginationQuery.EventGID);
            }

            if (request.PaginationQuery.LeaderGID != null)
            {
                result = result.Where(e => e.LeaderGID == request.PaginationQuery.LeaderGID);
            }

            var totalCount = result.Count();

            if (!request.PaginationQuery.GetAll())
            {
                result = result.Skip((request.PaginationQuery.PageNumber - 1) * request.PaginationQuery.PageSize).Take(request.PaginationQuery.PageSize);
            }

            return new GetAllEntitesReponse<GroupDTO> { Items = _mapper.Map<IEnumerable<GroupDTO>>(result), TotalCount = totalCount };
        }
    }
}
