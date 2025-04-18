using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Clear;

namespace Gateway.Infrastructure.Services.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IOperationsGateway _operationsGateway;

        public OperationsService(IOperationsGateway operationsGateway)
        {
            _operationsGateway = operationsGateway;
        }

        public async Task<GetAllEntitesReponse<ClearEvent>> GetClearEvents(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var result = new GetAllEntitesReponse<ClearEvent>();

            var response = await _operationsGateway.GetSortedEvents(paginationQuery, cancellationToken, token);

            result.TotalCount = response.TotalCount;

            foreach (var item in response.Items)
            {
                var dispatcher = await _operationsGateway.GetOperationWorkerByGID(item.DispatcherGID, cancellationToken, token);
                var coordinator = await _operationsGateway.GetOperationWorkerByGID(item.CoordinatorGID, cancellationToken, token);

                var clearEvent = new ClearEvent
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
