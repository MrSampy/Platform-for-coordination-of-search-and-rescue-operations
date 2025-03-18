using AutoMapper;
using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.OperationTaskCommands.Update
{
    public class UpdateOperationTaskCommandHandler : IRequestHandler<UpdateOperationTaskCommand>
    {
        private readonly IRepository<OperationTask> _operationTaskRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<OperationTaskStatus> _taskStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationTask> _cacheService;
        private readonly IMapper _mapper;

        public UpdateOperationTaskCommandHandler(
            IRepository<OperationTask> operationTaskRepository,
            IRepository<Group> groupRepository,
            IRepository<OperationTaskStatus> taskStatusRepository,
            IUnitOfWork unitOfWork,
            ICacheService<OperationTask> cacheService,
            IMapper mapper)
        {
            _operationTaskRepository = operationTaskRepository;
            _groupRepository = groupRepository;
            _taskStatusRepository = taskStatusRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task Handle(UpdateOperationTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operationTaskRepository.GetByGidAsync(request.OperationTask.GID, cancellationToken);
            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationTask), request.OperationTask.GID.ToString()));
            }

            var group = await _groupRepository.GetByGidAsync(request.OperationTask.GroupGID, cancellationToken);
            if (group == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Group), request.OperationTask.GroupGID.ToString()));
            }

            var taskStatus = await _taskStatusRepository.GetByGidAsync(request.OperationTask.TaskStatusGID, cancellationToken);
            if (taskStatus == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationTaskStatus), request.OperationTask.TaskStatusGID.ToString()));
            }

            var mappedEntity = _mapper.Map<OperationTask>(request.OperationTask);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

            await _operationTaskRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();
        }
    }
}
