using AutoMapper;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Detail;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Update;
using Gateway.DTO.DTOs.Volunteers;
using Gateway.DTO.Exceptions;

namespace Gateway.Infrastructure.Services.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IMapper _mapper;
        private readonly IAuthGateway _authGateway;
        private readonly IVolunteersGateway _volunteerGateway;

        public OperationsService(IOperationsGateway operationsGateway, IAuthGateway authGateway, IVolunteersGateway volunteersGateway, IMapper mapper)
        {
            _operationsGateway = operationsGateway;
            _mapper = mapper;
            _authGateway = authGateway;
            _volunteerGateway = volunteersGateway;
        }
        public async Task<IEnumerable<OperationTaskDTO>> GetOperationTasksByGroupGID(Guid groupGID, CancellationToken cancellationToken, string token)
        {
            var operationTasks = await _operationsGateway.GetOperationTasks(new PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);

            return operationTasks.Where(op => op.GroupGID == groupGID);
        }

        public async Task<GetAllEntitesReponse<GroupDetails>> GetGroupsByDispatcherGID(GetGroupsByDispatcherGIDRequest request, CancellationToken cancellationToken, string token)
        {
            var allGroups = await _operationsGateway.GetGroups(new PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);

            var events = await _operationsGateway.GetSortedEvents(new EventPaginationQuery { PageNumber = 0, PageSize = 0, DispatcherGID = request.DispatcherGID }, cancellationToken, token);

            var result = new List<GroupDetails>();

            foreach (var eventDTO in events.Items)
            {
                result.AddRange(await GetGroupsByEventGID(eventDTO.GID, cancellationToken, token));
            }

            var totalCount = result.Count();

            if (!request.GetAll())
            {
                result = (result.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)).ToList();
            }

            return new GetAllEntitesReponse<GroupDetails>() { Items = result, TotalCount = totalCount };
        }


        public async Task<GetAllEntitesReponse<GroupDetails>> GetGroupsDetails(GroupPaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var sortedGroups = await _operationsGateway.GetSortedGroups(paginationQuery, cancellationToken, token);

            var tasks = sortedGroups.Items
               .Select(group => ConvertToDetailGroup(group, token));

            var groups = await Task.WhenAll(tasks);

            return new GetAllEntitesReponse<GroupDetails>
            {
                Items = groups == null ? new List<GroupDetails>() : groups.ToList(),
                TotalCount = sortedGroups.TotalCount
            };
        }
        public async Task DeleteEvent(Guid gid, string token)
        {
            var volunteersEvents = await _volunteerGateway.GetVolunteersByEventGID(gid, CancellationToken.None, token);
            volunteersEvents ??= new List<VolunteersEventsDTO>();

            var resorceEvents = await _operationsGateway.GetResourcesByEventGID(gid, CancellationToken.None, token);
            resorceEvents ??= new List<ResourcesEventDTO>();

            var groups = ((await _operationsGateway.GetGroups(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)) ?? new List<GroupDTO>()).Where(g => g.EventGID == gid);

            foreach (var volunteersEvent in volunteersEvents)
            {
                await _volunteerGateway.RemoveVolunteerFromEvent(volunteersEvent.GID, token);
            }

            foreach (var resorceEvent in resorceEvents)
            {
                await _operationsGateway.DeleteResourcesEvent(resorceEvent.GID, token);
            }

            foreach (var group in groups)
            {
                await DeleteGroup(group.GID, token);
            }

            await _operationsGateway.DeleteEvent(gid, token);
        }
        public async Task DeleteGroup(Guid gid, string token)
        {
            var volunteersGroups = await _volunteerGateway.GetVolunteersByGroupGID(gid, CancellationToken.None, token);
            volunteersGroups ??= new List<VolunteersGroupsDTO>();

            var operationTasks = await _operationsGateway.GetOperationTasks(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token);
            operationTasks = operationTasks == null ? new List<OperationTaskDTO>() : operationTasks.Where(vg => vg.GroupGID == gid);

            foreach (var volunteersGroup in volunteersGroups)
            {
                await _volunteerGateway.RemoveVolunteerFromGroup(volunteersGroup.GID, token);
            }

            foreach (var operationTask in operationTasks)
            {
                await _operationsGateway.DeleteOperationTask(operationTask.GID, token);
            }

            await _operationsGateway.DeleteGroup(gid, token);
        }

        public async Task<IEnumerable<GroupDetails>> GetGroupsByEventGID(Guid eventGID, CancellationToken cancellationToken, string token)
        {
            var eventDTO = await _operationsGateway.GetEventByGID(eventGID, cancellationToken, token);

            var groups = await _operationsGateway.GetGroups(new PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);
            var tasks = groups
                .Where(g => g.EventGID == eventGID)
                .Select(group => ConvertToDetailGroup(group, token, eventDTO));

            return await Task.WhenAll(tasks);
        }

        public async Task<GroupDetails> ConvertToDetailGroup(GroupDTO group, string token, EventDTO? eventDTO = null)
        {
            eventDTO ??= await _operationsGateway.GetEventByGID(group.EventGID, CancellationToken.None, token);

            var result = new GroupDetails
            {
                GID = group.GID,
                Name = group.Name,
                EventGID = group.EventGID,
                LeaderGID = group.LeaderGID,
                EventName = eventDTO.Name
            };

            if (group.LeaderGID != null)
            {
                var leader = await _volunteerGateway.GetVolunteerByGID(group.LeaderGID.Value, CancellationToken.None, token);
                result.LeaderName = $"{leader.Name} {leader.Surname} {leader.SecondName}";
            }

            return result;
        }


        public async Task<GetAllEntitesReponse<MessageDetails>> GetMessages(MessagePaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var messges = await _operationsGateway.GetMessages(paginationQuery, cancellationToken, token);

            var resultMessages = new List<MessageDetails>();

            foreach (var message in messges.Items)
            {
                var messageDetail = _mapper.Map<MessageDetails>(message);

                var sender = await _operationsGateway.GetOperationWorkerByGID(message.From, cancellationToken, token);
                var receiver = await _operationsGateway.GetOperationWorkerByGID(message.To, cancellationToken, token);
                var eventDTO = await _operationsGateway.GetEventByGID(message.EventGID, cancellationToken, token);

                messageDetail.Sender = $"{sender.Name} {sender.Surname} {sender.SecondName}";
                messageDetail.Receiver = $"{receiver.Name} {receiver.Surname} {receiver.SecondName}";
                messageDetail.EventName = eventDTO.Name;

                resultMessages.Add(messageDetail);
            }

            return new GetAllEntitesReponse<MessageDetails>
            {
                Items = resultMessages,
                TotalCount = messges.TotalCount
            };
        }

        public async Task<OperationWorkerDTO?> GetWorkerByUserGID(Guid userGID, CancellationToken cancellationToken, string token)
        {
            var operationWorkers = await _operationsGateway.GetOperationWorkers(new PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);

            return operationWorkers.FirstOrDefault(x => x.UserGID == userGID);
        }

        public async Task<IEnumerable<OperationWorkerDTO>> GetWorkersByRole(GetOperationWorkersByRoleName request, CancellationToken cancellationToken, string token)
        {
            if (request == null || string.IsNullOrEmpty(request.RoleName))
                throw new ArgumentNullException(nameof(request));

            var users = _authGateway.GetAllUserIdsByRole(request.RoleName, cancellationToken, token);

            var operationWorkers = await _operationsGateway.GetOperationWorkers(new PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);

            return operationWorkers.Where(x => users.Any(u => u.ToLower() == x.UserGID.ToString().ToLower()));
        }

        public async Task<EventDTO> CreateEvent(CreateEventRequest request, string token)
        {
            var user = _authGateway.GetByGID(request.UserGID, CancellationToken.None, token);

            if (user == null)
                throw new ServiceException("User not found");

            var isDispatcher = user.Roles.Any(x => x.Name == SharedConstants.DispatcherRoleName);

            if (!isDispatcher)
                throw new ServiceException("User is not a dispatcher");

            var opeationWorkers = await _operationsGateway.GetOperationWorkers(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token);

            var dispatcher = opeationWorkers.FirstOrDefault(x => x.UserGID == request.UserGID);

            if (dispatcher == null)
                throw new ServiceException("Dispatcher not found");

            var createEventRequestDTO = new CreateEventDTO
            {
                Name = request.Name,
                DispatcherGID = dispatcher.GID,
                CoordinatorGID = request.CoordinatorGID,
                EventStatusGID = SharedConstants.EventStatusCreated,
                EventTypeGID = request.EventTypeGID,
                DistrictGID = request.DistrictGID,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
            };

            var eventDTO = await _operationsGateway.CreateEvent(createEventRequestDTO, token);

            return eventDTO;
        }

        public async Task EventStatusChange(EventStatusChangeRequest request, string token)
        {
            var eventDTO = await _operationsGateway.GetEventByGID(request.EventGID, CancellationToken.None, token);

            if (eventDTO != null && SharedConstants.EventStatuses.Any(e => e.GID == request.EventStatusGID))
            {
                eventDTO.EventStatusGID = request.EventStatusGID;
                eventDTO.Note = request.Note;
                var updatedEvent = _mapper.Map<UpdateEventDTO>(eventDTO);

                await _operationsGateway.UpdateEvent(updatedEvent, token);
            }
        }

        public async Task<GetAllEntitesReponse<EventDetails>> GetEventsDetail(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var result = new GetAllEntitesReponse<EventDetails>();

            var response = await _operationsGateway.GetSortedEvents(paginationQuery, cancellationToken, token);

            result.TotalCount = response.TotalCount;

            foreach (var item in response.Items)
            {
                result.Items.Add(await ConvertToDetailEvent(item, cancellationToken, token));
            }

            return result;
        }

        private async Task<EventDetails> ConvertToDetailEvent(EventDTO eventDTO, CancellationToken cancellationToken, string token)
        {
            var dispatcher = await _operationsGateway.GetOperationWorkerByGID(eventDTO.DispatcherGID, cancellationToken, token);
            var coordinator = await _operationsGateway.GetOperationWorkerByGID(eventDTO.CoordinatorGID, cancellationToken, token);

            var clearEvent = _mapper.Map<EventDetails>(eventDTO);

            clearEvent.Dispatcher = $"{dispatcher.Name} {dispatcher.Surname} {dispatcher.SecondName}";
            clearEvent.Coordinator = $"{coordinator.Name} {coordinator.Surname} {coordinator.SecondName}";
            clearEvent.EventStatus = SharedConstants.EventStatuses.FirstOrDefault(x => x.GID == eventDTO.EventStatusGID)?.Name!;
            clearEvent.EventType = SharedConstants.EventTypes.FirstOrDefault(x => x.GID == eventDTO.EventTypeGID)?.Name!;
            clearEvent.District = SharedConstants.Districts.FirstOrDefault(x => x.GID == eventDTO.DistrictGID)?.Name!;

            return clearEvent;
        }
    }
}
