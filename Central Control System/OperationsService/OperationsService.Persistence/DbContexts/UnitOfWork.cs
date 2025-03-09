using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.Repositories;

namespace OperationsService.Persistence.DbContexts
{
    public sealed class UnitOfWork(OperationsDbContext dbContext) : IUnitOfWork
    {
        private readonly OperationsDbContext dbContext = dbContext;

        private EventRepository _eventRepository;
        private EventStatusRepository _eventStatusRepository;
        private EventTypeRepository _eventTypeRepository;
        private GroupRepository _groupRepository;
        private OperationTaskRepository _operationTaskRepository;
        private OperationTaskStatusRepository _operationTaskStatusRepository;
        private OperationWorkerRepository _operationWorkerRepository;
        private ResourcesEventRepository _resourcesEventRepository;

        public IRepository<Event> EventRepository => _eventRepository ??= new EventRepository(dbContext);
        public IRepository<EventStatus> EventStatusRepository => _eventStatusRepository ??= new EventStatusRepository(dbContext);
        public IRepository<EventType> EventTypeRepository => _eventTypeRepository ??= new EventTypeRepository(dbContext);
        public IRepository<Group> GroupRepository => _groupRepository ??= new GroupRepository(dbContext);
        public IRepository<OperationTask> OperationTaskRepository => _operationTaskRepository ??= new OperationTaskRepository(dbContext);
        public IRepository<OperationTaskStatus> OperationTaskStatusRepository => _operationTaskStatusRepository ??= new OperationTaskStatusRepository(dbContext);
        public IRepository<OperationWorker> OperationWorkerRepository => _operationWorkerRepository ??= new OperationWorkerRepository(dbContext);
        public IRepository<ResourcesEvent> ResourcesEventRepository => _resourcesEventRepository ??= new ResourcesEventRepository(dbContext);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
