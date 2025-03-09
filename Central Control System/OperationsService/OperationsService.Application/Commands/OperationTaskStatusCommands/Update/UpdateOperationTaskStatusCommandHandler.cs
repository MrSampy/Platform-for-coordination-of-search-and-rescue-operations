using AutoMapper;
using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.OperationTaskStatusCommands.Update
{
    public class UpdateOperationTaskStatusCommandHandler : IRequestHandler<UpdateOperationTaskStatusCommand>
    {
        private readonly IRepository<OperationTaskStatus> _operationTaskStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationTaskStatus> _cacheService;
        private readonly IMapper _mapper;

        public UpdateOperationTaskStatusCommandHandler(IRepository<OperationTaskStatus> operationTaskStatusRepository, IUnitOfWork unitOfWork, ICacheService<OperationTaskStatus> cacheService, IMapper mapper)
        {
            _operationTaskStatusRepository = operationTaskStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task Handle(UpdateOperationTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operationTaskStatusRepository.GetByGidAsync(request.OperationTaskStatus.GID, cancellationToken);
            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationTaskStatus), request.OperationTaskStatus.GID.ToString()));
            }

            var mappedEntity = _mapper.Map<OperationTaskStatus>(request.OperationTaskStatus);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.Now;

            await _operationTaskStatusRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();
        }
    }
}
