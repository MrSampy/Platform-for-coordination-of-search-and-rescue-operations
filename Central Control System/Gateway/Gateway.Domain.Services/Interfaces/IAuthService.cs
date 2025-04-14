using Gateway.DTO.DTOs.Auth;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenInfoDTO> RegisterWorker(RegisterWorkerModel model);
    }
}