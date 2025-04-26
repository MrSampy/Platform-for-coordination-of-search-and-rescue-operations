using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IOperationsGateway
    {
        #region Message
        Task<GetAllEntitesReponse<MessageDTO>> GetMessages(MessagePaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<MessageDTO> GetMessageByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<MessageDTO> CreateMessage(CreateMessageDTO Message, string token);
        Task ReadMessage(Guid gid, string token);
        Task DeleteMessage(Guid gid, string token);
        #endregion

        #region Event
        Task<IEnumerable<EventDTO>> GetEvents(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<GetAllEntitesReponse<EventDTO>> GetSortedEvents(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<IEnumerable<EventDTO>> GetByStatusGID(Guid eventStatusGID, CancellationToken cancellationToken, string token);
        Task<EventDTO> GetEventByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<EventDTO> CreateEvent(CreateEventDTO Event, string token);
        Task UpdateEvent(UpdateEventDTO Event, string token);
        Task DeleteEvent(Guid gid, string token);
        #endregion

        #region EventStatus
        Task<IEnumerable<EventStatusDTO>> GetEventStatuss(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<EventStatusDTO> GetEventStatusByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<EventStatusDTO> CreateEventStatus(CreateEventStatusDTO EventStatus, string token);
        Task UpdateEventStatus(UpdateEventStatusDTO EventStatus, string token);
        Task DeleteEventStatus(Guid gid, string token);
        #endregion

        #region EventType
        Task<IEnumerable<EventTypeDTO>> GetEventTypes(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<EventTypeDTO> GetEventTypeByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<EventTypeDTO> CreateEventType(CreateEventTypeDTO EventType, string token);
        Task UpdateEventType(UpdateEventTypeDTO EventType, string token);
        Task DeleteEventType(Guid gid, string token);
        #endregion

        #region Group
        Task<IEnumerable<GroupDTO>> GetGroups(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<GroupDTO> GetGroupByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<GetAllEntitesReponse<GroupDTO>> GetSortedGroups(GroupPaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<GroupDTO> CreateGroup(CreateGroupDTO Group, string token);
        Task UpdateGroup(UpdateGroupDTO Group, string token);
        Task DeleteGroup(Guid gid, string token);
        #endregion

        #region OperationTask
        Task<IEnumerable<OperationTaskDTO>> GetOperationTasks(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<OperationTaskDTO> GetOperationTaskByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<OperationTaskDTO> CreateOperationTask(CreateOperationTaskDTO OperationTask, string token);
        Task UpdateOperationTask(UpdateOperationTaskDTO OperationTask, string token);
        Task DeleteOperationTask(Guid gid, string token);
        #endregion

        #region OperationTaskStatus
        Task<IEnumerable<OperationTaskStatusDTO>> GetOperationTaskStatuss(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<OperationTaskStatusDTO> GetOperationTaskStatusByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<OperationTaskStatusDTO> CreateOperationTaskStatus(CreateOperationTaskStatusDTO OperationTaskStatus, string token);
        Task UpdateOperationTaskStatus(UpdateOperationTaskStatusDTO OperationTaskStatus, string token);
        Task DeleteOperationTaskStatus(Guid gid, string token);
        #endregion

        #region OperationWorker
        Task<IEnumerable<OperationWorkerDTO>> GetOperationWorkers(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<OperationWorkerDTO> GetOperationWorkerByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<OperationWorkerDTO> CreateOperationWorker(CreateOperationWorkerDTO OperationWorker, string token);
        Task UpdateOperationWorker(UpdateOperationWorkerDTO OperationWorker, string token);
        Task DeleteOperationWorker(Guid gid, string token);
        #endregion

        #region ResourcesEvent
        Task<IEnumerable<ResourcesEventDTO>> GetResourcesEvents(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<IEnumerable<ResourcesEventDTO>> GetResourcesByEventGID(Guid eventGID, CancellationToken cancellationToken, string token);
        Task<IEnumerable<ResourcesEventDTO>> GetEventsByResourceGID(Guid resourceGID, CancellationToken cancellationToken, string token);
        Task<ResourcesEventDTO> GetResourcesEventByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<ResourcesEventDTO> CreateResourcesEvent(CreateResourcesEventDTO ResourcesEvent, string token);
        Task UpdateResourcesEvent(UpdateResourcesEventDTO ResourcesEvent, string token);
        Task DeleteResourcesEvent(Guid gid, string token);
        #endregion
    }
}
