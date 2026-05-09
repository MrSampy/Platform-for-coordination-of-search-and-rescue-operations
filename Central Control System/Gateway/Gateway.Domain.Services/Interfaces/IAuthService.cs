using Gateway.DTO.DTOs.Auth;
using Gateway.DTO.DTOs.Common;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterWorker(RegisterWorkerModel model, string token);
        IsExistModel IsUserWithSuchName(string name, CancellationToken cancellationToken);
        IsExistModel IsUserWithSuchEmail(string email, CancellationToken cancellationToken);
    }
}