using AuthService.API.Core.DTO;

namespace AuthService.API.Core.Interfaces
{
    public interface IUserSevice
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO? GetByGID(Guid gid);
        UserDTO? GetByUserName(string userName);
        UserDTO? GetByEmail(string email);
        IEnumerable<string> GetAllUserIdsByRole(string roleName);
        Task<UserDTO> UpdateUserRoles(UserDTO query);
    }
}
