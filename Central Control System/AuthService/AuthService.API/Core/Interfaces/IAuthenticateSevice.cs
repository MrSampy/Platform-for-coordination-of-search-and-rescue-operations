using AuthService.API.Core.DTO;
using AuthService.API.Core.Models;

namespace AuthService.API.Core.Interfaces
{
    public interface IAuthenticateSevice
    {
        Task<TokenInfoDTO> Login(LoginModel model);
        Task<UserDTO> Register(RegisterModel model);
        Task<UserDTO> RegisterAdmin(RegisterModel model);
        Task<UserDTO> RegisterCoordinator(RegisterModel model);
        Task<UserDTO> RegisterDispatcher(RegisterModel model);
        Task<MeResponse> Me(HttpContext httpContext);
    }
}
