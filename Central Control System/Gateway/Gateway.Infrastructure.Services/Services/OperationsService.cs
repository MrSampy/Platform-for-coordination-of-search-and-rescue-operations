using AutoMapper;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations.Clear;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Infrastructure.Services.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IMapper _mapper;

        public OperationsService(IOperationsGateway operationsGateway, IMapper mapper)
        {
            _operationsGateway = operationsGateway;
            _mapper = mapper;
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
