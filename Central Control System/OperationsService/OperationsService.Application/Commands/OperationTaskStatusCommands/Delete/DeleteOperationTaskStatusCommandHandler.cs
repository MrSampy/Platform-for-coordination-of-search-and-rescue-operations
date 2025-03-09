using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.OperationTaskStatusCommands.Delete
{
    public class DeleteOperationTaskStatusCommandHandler : IRequestHandler<DeleteOperationTaskStatusCommand>
    {
        private readonly IRepository<OperationTaskStatus> _operationTaskStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationTaskStatus> _cacheService;

        public DeleteOperationTaskStatusCommandHandler(IRepository<OperationTaskStatus> operationTaskStatusRepository, IUnitOfWork unitOfWork, ICacheService<OperationTaskStatus> cacheService)
        {
            _operationTaskStatusRepository = operationTaskStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteOperationTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operationTaskStatusRepository.GetByGidAsync(request.GID, cancellationToken);
            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationTaskStatus), request.GID.ToString()));
            }

            await _operationTaskStatusRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();
        }
    }
}
