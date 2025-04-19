using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationTaskQueries.GetAll
{
    public class GetAllOperationTasksQueryHandler : IRequestHandler<GetAllOperationTasksQuery, IEnumerable<OperationTaskDTO>>
    {
        private readonly IRepository<OperationTask> _operationTaskRepository;
        private readonly ICacheService<OperationTask> _cacheService;
        private readonly IMapper _mapper;

        public GetAllOperationTasksQueryHandler(IRepository<OperationTask> operationTaskRepository, ICacheService<OperationTask> cacheService, IMapper mapper)
        {
            _operationTaskRepository = operationTaskRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OperationTaskDTO>> Handle(GetAllOperationTasksQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllOperationTasksQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";
            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<OperationTaskDTO>>(cachedEntities);
            }

            var result = await _operationTaskRepository.GetAllAsync(cancellationToken, request.PaginationQuery.GetAll() ? null : request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<OperationTaskDTO>>(result);
        }
    }
}
