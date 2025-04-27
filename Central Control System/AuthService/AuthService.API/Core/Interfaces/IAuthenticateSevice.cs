using AuthService.API.Core.DTO;
using AuthService.API.Core.Models;

namespace AuthService.API.Core.Interfaces
{
    public interface IAuthenticateSevice
    {
        Task<LoginResponse> Login(LoginModel model, bool use2FA);
        Task<UserDTO> Register(RegisterModel model);
        Task<UserDTO> RegisterAdmin(RegisterModel model);
        Task<UserDTO> RegisterCoordinator(RegisterModel model);
        Task<UserDTO> RegisterDispatcher(RegisterModel model);
        Task<MeResponse> Me(HttpContext httpContext);
        Task<GetAuthenticatorKeyResponse> GetAuthenticatorKey(LoginModel model);
        Task<TokenInfoDTO> GetToken(GetTokenRequest model, bool use2FA);
    }
}
