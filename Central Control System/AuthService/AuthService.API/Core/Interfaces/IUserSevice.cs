using AuthService.API.Core.DTO;

namespace AuthService.API.Core.Interfaces
{
    public interface IUserSevice
    {
        IEnumerable<UserDTO> GetAllUsers();
        IEnumerable<string> GetAllUserIdsByRole(string roleName);
        Task<UserDTO> UpdateUserRoles(UserDTO query);
    }
}
