using Gateway.DTO.DTOs.Auth;
using Gateway.DTO.DTOs.Common;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenInfoDTO> RegisterWorker(RegisterWorkerModel model);
        IsExistModel IsUserWithSuchName(string name, CancellationToken cancellationToken);
        IsExistModel IsUserWithSuchEmail(string email, CancellationToken cancellationToken);
    }
}