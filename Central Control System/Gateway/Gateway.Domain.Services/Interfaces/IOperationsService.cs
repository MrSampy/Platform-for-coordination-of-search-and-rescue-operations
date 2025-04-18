using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Clear;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IOperationsService
    {
        Task<GetAllEntitesReponse<ClearEvent>> GetClearEvents(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
    }
}
