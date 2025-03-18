using AutoMapper;
using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.OperationWorkerCommands.Update
{
    public class UpdateOperationWorkerCommandHandler : IRequestHandler<UpdateOperationWorkerCommand>
    {
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationWorker> _cacheService;
        private readonly IMapper _mapper;

        public UpdateOperationWorkerCommandHandler(
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

        public async Task Handle(UpdateOperationWorkerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operationWorkerRepository.GetByGidAsync(request.OperationWorker.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), request.OperationWorker.GID.ToString()));
            }

            var mappedEntity = _mapper.Map<OperationWorker>(request.OperationWorker);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

            await _operationWorkerRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
