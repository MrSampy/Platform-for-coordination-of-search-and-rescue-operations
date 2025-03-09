using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.OperationTaskCommands.Delete
{
    public class DeleteOperationTaskCommandHandler : IRequestHandler<DeleteOperationTaskCommand>
    {
        private readonly IRepository<OperationTask> _operationTaskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationTask> _cacheService;

        public DeleteOperationTaskCommandHandler(IRepository<OperationTask> operationTaskRepository, IUnitOfWork unitOfWork, ICacheService<OperationTask> cacheService)
        {
            _operationTaskRepository = operationTaskRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteOperationTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operationTaskRepository.GetByGidAsync(request.GID, cancellationToken);
            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationTask), request.GID.ToString()));
            }

            await _operationTaskRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();
        }
    }
}
