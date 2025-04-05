using AuthService.API.Core.DTO;
using AuthService.API.Core.Models;

namespace AuthService.API.Core.Interfaces
{
    public interface IAuthenticateSevice
    {
        Task<string> Register(RegisterModel model);
        Task RegisterAdmin(RegisterModel model);
        Task<TokenInfoDTO> Login(LoginModel model);
        Task RegisterCoordinator(RegisterModel model);
        Task RegisterDispatcher(RegisterModel model);
    }
}
