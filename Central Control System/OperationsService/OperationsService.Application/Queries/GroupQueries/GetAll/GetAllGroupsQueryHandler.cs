using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.GroupQueries.GetAll
{
    public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<GroupDTO>>
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly ICacheService<Group> _cacheService;
        private readonly IMapper _mapper;

        public GetAllGroupsQueryHandler(IRepository<Group> groupRepository, ICacheService<Group> cacheService, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDTO>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllGroupsQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";
            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<GroupDTO>>(cachedEntities);
            }

            var result = await _groupRepository.GetAllAsync(cancellationToken, request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<GroupDTO>>(result);
        }
    }
}
