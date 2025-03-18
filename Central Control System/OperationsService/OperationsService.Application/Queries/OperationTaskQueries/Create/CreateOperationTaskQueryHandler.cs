using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationTaskQueries.Create
{
    public class CreateOperationTaskQueryHandler : IRequestHandler<CreateOperationTaskQuery, OperationTaskDTO>
    {
        private readonly IRepository<OperationTask> _operationTaskRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<OperationTaskStatus> _taskStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationTask> _cacheService;
        private readonly IMapper _mapper;

        public CreateOperationTaskQueryHandler(
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

        public async Task<OperationTaskDTO> Handle(CreateOperationTaskQuery request, CancellationToken cancellationToken)
        {
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

            var operationTask = _mapper.Map<OperationTask>(request.OperationTask);
            operationTask.GID = Guid.NewGuid();
            operationTask.CreatedAt = operationTask.UpdatedAt = DateTime.UtcNow;
            await _operationTaskRepository.AddAsync(operationTask);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();

            return _mapper.Map<OperationTaskDTO>(operationTask);
        }
    }

}
