using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationTaskStatusQueries.Create
{
    public class CreateOperationTaskStatusQueryHandler : IRequestHandler<CreateOperationTaskStatusQuery, OperationTaskStatusDTO>
    {
        private readonly IRepository<OperationTaskStatus> _operationTaskStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationTaskStatus> _cacheService;
        private readonly IMapper _mapper;

        public CreateOperationTaskStatusQueryHandler(IRepository<OperationTaskStatus> operationTaskStatusRepository, IUnitOfWork unitOfWork, ICacheService<OperationTaskStatus> cacheService, IMapper mapper)
        {
            _operationTaskStatusRepository = operationTaskStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<OperationTaskStatusDTO> Handle(CreateOperationTaskStatusQuery request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<OperationTaskStatus>(request.OperationTaskStatus);
            entity.GID = Guid.NewGuid();
            entity.CreatedAt = entity.UpdatedAt = DateTime.Now;
            await _operationTaskStatusRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();

            return _mapper.Map<OperationTaskStatusDTO>(entity);
        }
    }
}
