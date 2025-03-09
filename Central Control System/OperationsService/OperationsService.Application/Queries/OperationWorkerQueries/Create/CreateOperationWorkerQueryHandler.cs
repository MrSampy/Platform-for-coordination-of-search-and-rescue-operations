using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationWorkerQueries.Create
{
    public class CreateOperationWorkerQueryHandler : IRequestHandler<CreateOperationWorkerQuery, OperationWorkerDTO>
    {
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationWorker> _cacheService;
        private readonly IMapper _mapper;

        public CreateOperationWorkerQueryHandler(
            IRepository<OperationWorker> operationWorkerRepository,
            IUnitOfWork unitOfWork,
            ICacheService<OperationWorker> cacheService,
            IMapper mapper)
        {
            _operationWorkerRepository = operationWorkerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<OperationWorkerDTO> Handle(CreateOperationWorkerQuery request, CancellationToken cancellationToken)
        {
            var operationWorker = _mapper.Map<OperationWorker>(request.OperationWorker);
            operationWorker.GID = Guid.NewGuid();
            operationWorker.CreatedAt = operationWorker.UpdatedAt = DateTime.Now;
            await _operationWorkerRepository.AddAsync(operationWorker);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<OperationWorkerDTO>(operationWorker);
        }
    }

}
