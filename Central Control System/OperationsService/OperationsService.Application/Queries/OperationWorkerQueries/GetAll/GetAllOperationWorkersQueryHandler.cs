using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationWorkerQueries.GetAll
{
    public class GetAllOperationWorkersQueryHandler : IRequestHandler<GetAllOperationWorkersQuery, IEnumerable<OperationWorkerDTO>>
    {
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly ICacheService<OperationWorker> _cacheService;
        private readonly IMapper _mapper;

        public GetAllOperationWorkersQueryHandler(
            IRepository<OperationWorker> operationWorkerRepository,
            ICacheService<OperationWorker> cacheService,
            IMapper mapper)
        {
            _operationWorkerRepository = operationWorkerRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OperationWorkerDTO>> Handle(GetAllOperationWorkersQuery request, CancellationToken cancellationToken)
        {
            if (!request.PaginationQuery.IsValid())
            {
                throw new OperationsServiceException(Constants.InvalidPaginationQueryParametersException);
            }

            string cacheKey = $"{nameof(GetAllOperationWorkersQueryHandler)}:{request.PaginationQuery.PageNumber}:{request.PaginationQuery.PageSize}";

            var cachedEntities = _cacheService.Get(cacheKey);
            if (cachedEntities != null)
            {
                return _mapper.Map<IEnumerable<OperationWorkerDTO>>(cachedEntities);
            }

            var result = await _operationWorkerRepository.GetAllAsync(cancellationToken, request.PaginationQuery.GetAll() ? null : request.PaginationQuery);
            _cacheService.Set(cacheKey, result.ToList());

            return _mapper.Map<IEnumerable<OperationWorkerDTO>>(result);
        }
    }
}
