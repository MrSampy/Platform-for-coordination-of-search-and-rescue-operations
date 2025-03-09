using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.OperationWorkerCommands.Delete
{
    public class DeleteOperationWorkerCommandHandler : IRequestHandler<DeleteOperationWorkerCommand>
    {
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationWorker> _cacheService;

        public DeleteOperationWorkerCommandHandler(
            IRepository<OperationWorker> operationWorkerRepository,
            IUnitOfWork unitOfWork,
            ICacheService<OperationWorker> cacheService)
        {
            _operationWorkerRepository = operationWorkerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteOperationWorkerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operationWorkerRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), request.GID.ToString()));
            }

            await _operationWorkerRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
