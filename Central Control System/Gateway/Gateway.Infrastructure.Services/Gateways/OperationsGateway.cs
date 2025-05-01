using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Infrastructure.Services.Gateways
{
    public class OperationsGateway : IOperationsGateway
    {
        private readonly IApiBuilder _apiBuilder;
        private readonly ICacheService<MessageDTO> _messageCache;
        private readonly ICacheService<EventDTO> _eventCache;
        private readonly ICacheService<EventStatusDTO> _eventStatusCache;
        private readonly ICacheService<EventTypeDTO> _eventTypeCache;
        private readonly ICacheService<GroupDTO> _groupCache;
        private readonly ICacheService<OperationTaskDTO> _operationTaskCache;
        private readonly ICacheService<OperationTaskStatusDTO> _operationTaskStatusCache;
        private readonly ICacheService<OperationWorkerDTO> _operationWorkerCache;
        private readonly ICacheService<ResourcesEventDTO> _resourcesEventCache;

        public OperationsGateway(IApiBuilder apiBuilder,
            ICacheService<MessageDTO> messageCache,
            ICacheService<EventDTO> eventCache,
            ICacheService<EventStatusDTO> eventStatusCache,
            ICacheService<EventTypeDTO> eventTypeCache,
            ICacheService<GroupDTO> groupCache,
            ICacheService<OperationTaskDTO> operationTaskCache,
            ICacheService<OperationTaskStatusDTO> operationTaskStatusCache,
            ICacheService<OperationWorkerDTO> operationWorkerCache,
            ICacheService<ResourcesEventDTO> resourcesEventCache)
        {
            _apiBuilder = apiBuilder;
            _messageCache = messageCache;
            _eventCache = eventCache;
            _eventStatusCache = eventStatusCache;
            _eventTypeCache = eventTypeCache;
            _groupCache = groupCache;
            _operationTaskCache = operationTaskCache;
            _operationTaskStatusCache = operationTaskStatusCache;
            _operationWorkerCache = operationWorkerCache;
            _resourcesEventCache = resourcesEventCache;
        }

        #region Message

        public async Task<GetAllEntitesReponse<MessageDTO>> GetMessages(MessagePaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _messageCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return new GetAllEntitesReponse<MessageDTO> { Items = cached, TotalCount = cached.Count };

            var result = await _apiBuilder.PostRequest<GetAllEntitesReponse<MessageDTO>>("operations/api/Message/get", paginationQuery, SharedConstants.OperationsService, cancellationToken, token);

            _messageCache.Set(paginationQuery.GetKey(), result.Items.ToList());
            return result;
        }

        public async Task<MessageDTO> GetMessageByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<MessageDTO>($"operations/api/Message/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<MessageDTO> CreateMessage(CreateMessageDTO message, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(MessageDTO));
            return await _apiBuilder.PostRequest<MessageDTO>("operations/api/Message", message, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task ReadMessage(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(MessageDTO));
            await _apiBuilder.PutRequestWithoutDeserializing($"operations/api/Message/read/{gid}", new { }, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteMessage(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(MessageDTO));
            await _apiBuilder.DeleteRequest($"operations/api/Message/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion


        #region Event

        public async Task<IEnumerable<EventDTO>> GetEvents(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _eventCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var url = $"operations/api/Event?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<EventDTO>>(url, SharedConstants.OperationsService, cancellationToken, token);

            _eventCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<GetAllEntitesReponse<EventDTO>> GetSortedEvents(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var result = await _apiBuilder.PostRequest<GetAllEntitesReponse<EventDTO>>("operations/api/Event/sort", paginationQuery, SharedConstants.OperationsService, cancellationToken, token);

            return result;
        }

        public async Task<IEnumerable<EventDTO>> GetByStatusGID(Guid eventStatusGID, CancellationToken cancellationToken, string token)
        {
            var cached = _eventCache.Get($"{nameof(GetByStatusGID)}:{eventStatusGID}");
            if (cached != null)
                return cached;

            var result = await _apiBuilder.GetRequest<IEnumerable<EventDTO>>($"operations/api/Event/by-eventstatus/{eventStatusGID}", SharedConstants.OperationsService, cancellationToken, token);

            _eventCache.Set($"{nameof(GetByStatusGID)}:{eventStatusGID}", result.ToList());
            return result;
        }

        public async Task<EventDTO> GetEventByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<EventDTO>($"operations/api/Event/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<EventDTO> CreateEvent(CreateEventDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventDTO));
            return await _apiBuilder.PostRequest<EventDTO>("operations/api/Event", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task UpdateEvent(UpdateEventDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("operations/api/Event", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteEvent(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventDTO));
            await _apiBuilder.DeleteRequest($"operations/api/Event/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion

        #region EventStatus

        public async Task<IEnumerable<EventStatusDTO>> GetEventStatuss(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _eventStatusCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var url = $"operations/api/EventStatus?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<EventStatusDTO>>(url, SharedConstants.OperationsService, cancellationToken, token);

            _eventStatusCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<EventStatusDTO> GetEventStatusByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<EventStatusDTO>($"operations/api/EventStatus/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<EventStatusDTO> CreateEventStatus(CreateEventStatusDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventStatusDTO));
            return await _apiBuilder.PostRequest<EventStatusDTO>("operations/api/EventStatus", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task UpdateEventStatus(UpdateEventStatusDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventStatusDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("operations/api/EventStatus", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteEventStatus(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventStatusDTO));
            await _apiBuilder.DeleteRequest($"operations/api/EventStatus/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion


        #region EventType

        public async Task<IEnumerable<EventTypeDTO>> GetEventTypes(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _eventTypeCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var url = $"operations/api/EventType?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<EventTypeDTO>>(url, SharedConstants.OperationsService, cancellationToken, token);

            _eventTypeCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<EventTypeDTO> GetEventTypeByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<EventTypeDTO>($"operations/api/EventType/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<EventTypeDTO> CreateEventType(CreateEventTypeDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventTypeDTO));
            return await _apiBuilder.PostRequest<EventTypeDTO>("operations/api/EventType", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task UpdateEventType(UpdateEventTypeDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventTypeDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("operations/api/EventType", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteEventType(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(EventTypeDTO));
            await _apiBuilder.DeleteRequest($"operations/api/EventType/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion


        #region Group

        public async Task<IEnumerable<GroupDTO>> GetGroups(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _groupCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var url = $"operations/api/Group?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<GroupDTO>>(url, SharedConstants.OperationsService, cancellationToken, token);

            _groupCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<GetAllEntitesReponse<GroupDTO>> GetSortedGroups(GroupPaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var result = await _apiBuilder.PostRequest<GetAllEntitesReponse<GroupDTO>>("operations/api/Group/sort", paginationQuery, SharedConstants.OperationsService, cancellationToken, token);

            return result;
        }

        public async Task<GroupDTO> GetGroupByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<GroupDTO>($"operations/api/Group/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<GroupDTO> CreateGroup(CreateGroupDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(GroupDTO));
            return await _apiBuilder.PostRequest<GroupDTO>("operations/api/Group", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task UpdateGroup(UpdateGroupDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(GroupDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("operations/api/Group", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteGroup(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(GroupDTO));
            await _apiBuilder.DeleteRequest($"operations/api/Group/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion


        #region OperationTask

        public async Task<IEnumerable<OperationTaskDTO>> GetOperationTasks(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _operationTaskCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var url = $"operations/api/OperationTask?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<OperationTaskDTO>>(url, SharedConstants.OperationsService, cancellationToken, token);

            _operationTaskCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<OperationTaskDTO> GetOperationTaskByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<OperationTaskDTO>($"operations/api/OperationTask/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<OperationTaskDTO> CreateOperationTask(CreateOperationTaskDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationTaskDTO));
            return await _apiBuilder.PostRequest<OperationTaskDTO>("operations/api/OperationTask", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task UpdateOperationTask(UpdateOperationTaskDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationTaskDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("operations/api/OperationTask", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteOperationTask(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationTaskDTO));
            await _apiBuilder.DeleteRequest($"operations/api/OperationTask/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion


        #region OperationTaskStatus

        public async Task<IEnumerable<OperationTaskStatusDTO>> GetOperationTaskStatuss(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _operationTaskStatusCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var url = $"operations/api/OperationTaskStatus?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<OperationTaskStatusDTO>>(url, SharedConstants.OperationsService, cancellationToken, token);

            _operationTaskStatusCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<OperationTaskStatusDTO> GetOperationTaskStatusByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<OperationTaskStatusDTO>($"operations/api/OperationTaskStatus/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<OperationTaskStatusDTO> CreateOperationTaskStatus(CreateOperationTaskStatusDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationTaskStatusDTO));
            return await _apiBuilder.PostRequest<OperationTaskStatusDTO>("operations/api/OperationTaskStatus", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task UpdateOperationTaskStatus(UpdateOperationTaskStatusDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationTaskStatusDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("operations/api/OperationTaskStatus", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteOperationTaskStatus(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationTaskStatusDTO));
            await _apiBuilder.DeleteRequest($"operations/api/OperationTaskStatus/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion


        #region OperationWorker

        public async Task<IEnumerable<OperationWorkerDTO>> GetOperationWorkers(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _operationWorkerCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var url = $"operations/api/OperationWorker?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<OperationWorkerDTO>>(url, SharedConstants.OperationsService, cancellationToken, token);

            _operationWorkerCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<OperationWorkerDTO> GetOperationWorkerByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<OperationWorkerDTO>($"operations/api/OperationWorker/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<OperationWorkerDTO> CreateOperationWorker(CreateOperationWorkerDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationWorkerDTO));
            return await _apiBuilder.PostRequest<OperationWorkerDTO>("operations/api/OperationWorker", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task UpdateOperationWorker(UpdateOperationWorkerDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationWorkerDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("operations/api/OperationWorker", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteOperationWorker(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(OperationWorkerDTO));
            await _apiBuilder.DeleteRequest($"operations/api/OperationWorker/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion


        #region ResourcesEvent

        public async Task<IEnumerable<ResourcesEventDTO>> GetResourcesEvents(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _resourcesEventCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var url = $"operations/api/ResourcesEvent?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<ResourcesEventDTO>>(url, SharedConstants.OperationsService, cancellationToken, token);

            _resourcesEventCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<IEnumerable<ResourcesEventDTO>> GetResourcesByEventGID(Guid eventGID, CancellationToken cancellationToken, string token)
        {
            var cached = _resourcesEventCache.Get($"{nameof(GetResourcesByEventGID)}:{eventGID}");
            if (cached != null)
                return cached;

            var result = await _apiBuilder.GetRequest<IEnumerable<ResourcesEventDTO>>($"operations/api/ResourcesEvent/by-event/{eventGID}", SharedConstants.OperationsService, cancellationToken, token);

            _resourcesEventCache.Set($"{nameof(GetResourcesByEventGID)}:{eventGID}", result.ToList());
            return result;
        }

        public async Task<IEnumerable<ResourcesEventDTO>> GetEventsByResourceGID(Guid resourceGID, CancellationToken cancellationToken, string token)
        {
            var cached = _resourcesEventCache.Get($"{nameof(GetEventsByResourceGID)}:{resourceGID}");
            if (cached != null)
                return cached;

            var result = await _apiBuilder.GetRequest<IEnumerable<ResourcesEventDTO>>($"operations/api/ResourcesEvent/by-resource/{resourceGID}", SharedConstants.OperationsService, cancellationToken, token);

            _resourcesEventCache.Set($"{nameof(GetEventsByResourceGID)}:{resourceGID}", result.ToList());
            return result;
        }

        public async Task<ResourcesEventDTO> GetResourcesEventByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<ResourcesEventDTO>($"operations/api/ResourcesEvent/{gid}", SharedConstants.OperationsService, cancellationToken, token);
        }

        public async Task<ResourcesEventDTO> CreateResourcesEvent(CreateResourcesEventDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(ResourcesEventDTO));
            return await _apiBuilder.PostRequest<ResourcesEventDTO>("operations/api/ResourcesEvent", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task UpdateResourcesEvent(UpdateResourcesEventDTO dto, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(ResourcesEventDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("operations/api/ResourcesEvent", dto, SharedConstants.OperationsService, CancellationToken.None, token);
        }

        public async Task DeleteResourcesEvent(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(ResourcesEventDTO));
            await _apiBuilder.DeleteRequest($"operations/api/ResourcesEvent/{gid}", SharedConstants.OperationsService, CancellationToken.None, token);
        }

        #endregion

    }
}
