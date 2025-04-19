using AutoMapper;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Clear;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Update;
using Gateway.DTO.Exceptions;

namespace Gateway.Infrastructure.Services.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IMapper _mapper;
        private readonly IAuthGateway _authGateway;

        public OperationsService(IOperationsGateway operationsGateway, IAuthGateway authGateway, IMapper mapper)
        {
            _operationsGateway = operationsGateway;
            _mapper = mapper;
            _authGateway = authGateway;
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
                var updatedEvent = _mapper.Map<UpdateEventDTO>(eventDTO);

                await _operationsGateway.UpdateEvent(updatedEvent, token);
            }
        }

        public async Task<GetAllEntitesReponse<DetailEvent>> GetClearEvents(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var result = new GetAllEntitesReponse<DetailEvent>();

            var response = await _operationsGateway.GetSortedEvents(paginationQuery, cancellationToken, token);

            result.TotalCount = response.TotalCount;

            foreach (var item in response.Items)
            {
                var dispatcher = await _operationsGateway.GetOperationWorkerByGID(item.DispatcherGID, cancellationToken, token);
                var coordinator = await _operationsGateway.GetOperationWorkerByGID(item.CoordinatorGID, cancellationToken, token);

                var clearEvent = new DetailEvent
                {
                    GID = item.GID,
                    Name = item.Name,
                    Dispatcher = $"{dispatcher.Name} {dispatcher.Surname} {dispatcher.SecondName}",
                    Coordinator = $"{coordinator.Name} {coordinator.Surname} {coordinator.SecondName}",
                    EventStatus = SharedConstants.EventStatuses.FirstOrDefault(x => x.GID == item.EventStatusGID)?.Name!,
                    EventType = SharedConstants.EventTypes.FirstOrDefault(x => x.GID == item.EventTypeGID)?.Name!,
                    District = SharedConstants.Districts.FirstOrDefault(x => x.GID == item.DistrictGID)?.Name!,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                };

                result.Items.Add(clearEvent);
            }

            return result;
        }
    }
}
