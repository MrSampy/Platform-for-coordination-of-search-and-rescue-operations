using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Detail;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Response;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IOperationsService
    {
        Task<GetAllEntitesReponse<DetailEvent>> GetClearEvents(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<IEnumerable<OperationWorkerDTO>> GetWorkersByRole(GetOperationWorkersByRoleName request, CancellationToken cancellationToken, string token);
        Task EventStatusChange(EventStatusChangeRequest request, string token);
        Task<EventDTO> CreateEvent(CreateEventRequest request, string token);
        Task<GetReportResponse> GenerateEventReport(Guid eventGID, CancellationToken cancellationToken, string token);
        Task<OperationWorkerDTO?> GetWorkerByUserGID(Guid userGID, CancellationToken cancellationToken, string token);
        Task<GetAllEntitesReponse<MessageDetail>> GetMessages(MessagePaginationQuery paginationQuery, CancellationToken cancellationToken, string token);

    }
}
