using OperationsService.Domain.Entities;

namespace OperationsService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Event> EventRepository { get; }
        IRepository<EventStatus> EventStatusRepository { get; }
        IRepository<EventType> EventTypeRepository { get; }
        IRepository<Group> GroupRepository { get; }
        IRepository<OperationTask> OperationTaskRepository { get; }
        IRepository<OperationTaskStatus> OperationTaskStatusRepository { get; }
        IRepository<OperationWorker> OperationWorkerRepository { get; }
        IRepository<ResourcesEvent> ResourcesEventRepository { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
