using Gateway.DTO.DTOs.Auth;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IAuthGateway
    {
        Task<UserDTO> Register(RegisterModel model);
        Task<UserDTO> RegisterAdmin(RegisterModel model, string token);
        Task<TokenInfoDTO> Login(LoginModel model);
        Task<MeResponse> Me(string token);
        Task<UserDTO> RegisterCoordinator(RegisterModel model, string token);
        Task<UserDTO> RegisterDispatcher(RegisterModel model);
        IEnumerable<RoleDTO> GetAllRoles(CancellationToken cancellationToken, string token);
        IEnumerable<UserDTO> GetAllUsers(CancellationToken cancellationToken, string token);
        UserDTO? GetByGID(Guid gid, CancellationToken cancellationToken, string token);
        UserDTO? GetByUserName(string userName, CancellationToken cancellationToken);
        UserDTO? GetByEmail(string email, CancellationToken cancellationToken);
        IEnumerable<string> GetAllUserIdsByRole(string roleName, CancellationToken cancellationToken, string token);
        Task<UserDTO> UpdateUserRoles(UserDTO query, string token);
    }
}
