using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations.Clear;
using Gateway.DTO.DTOs.Operations.Request;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IOperationsService
    {
        Task<GetAllEntitesReponse<ClearEvent>> GetClearEvents(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token);

        Task EventStatusChange(EventStatusChangeRequest request, string token);
    }
}
