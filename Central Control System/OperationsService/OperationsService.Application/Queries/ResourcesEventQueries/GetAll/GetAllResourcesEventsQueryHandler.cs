using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.ResourcesEventQueries.GetAll
{
    public class GetAllResourcesEventsQueryHandler : IRequestHandler<GetAllResourcesEventsQuery, IEnumerable<ResourcesEventDTO>>
    {
        private readonly IRepository<ResourcesEvent> _resourcesEventRepository;
        private readonly ICacheService<ResourcesEvent> _cacheService;
        private readonly IMapper _mapper;

        public GetAllResourcesEventsQueryHandler(
            IRepository<ResourcesEvent> resourcesEventRepository,
            ICacheService<ResourcesEvent> cacheService,
            IMapper mapper)
        {
            _resourcesEventRepository = resourcesEventRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResourcesEventDTO>> Handle(GetAllResourcesEventsQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllResourcesEventsQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<ResourcesEventDTO>>(cachedEntities);
            }

            var result = await _resourcesEventRepository.GetAllAsync(cancellationToken, request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<ResourcesEventDTO>>(result);
        }
    }

}
