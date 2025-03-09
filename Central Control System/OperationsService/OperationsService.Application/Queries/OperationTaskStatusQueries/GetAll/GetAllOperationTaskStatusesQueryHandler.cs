using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationTaskStatusQueries.GetAll
{
    public class GetAllOperationTaskStatusesQueryHandler : IRequestHandler<GetAllOperationTaskStatusesQuery, IEnumerable<OperationTaskStatusDTO>>
    {
        private readonly IRepository<OperationTaskStatus> _operationTaskStatusRepository;
        private readonly ICacheService<OperationTaskStatus> _cacheService;
        private readonly IMapper _mapper;

        public GetAllOperationTaskStatusesQueryHandler(IRepository<OperationTaskStatus> operationTaskStatusRepository, ICacheService<OperationTaskStatus> cacheService, IMapper mapper)
        {
            _operationTaskStatusRepository = operationTaskStatusRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OperationTaskStatusDTO>> Handle(GetAllOperationTaskStatusesQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllOperationTaskStatusesQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";
            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<OperationTaskStatusDTO>>(cachedEntities);
            }

            var result = await _operationTaskStatusRepository.GetAllAsync(cancellationToken, request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());
            return _mapper.Map<IEnumerable<OperationTaskStatusDTO>>(result);
        }
    }
}
