using AuthService.API.Core.DTO;

namespace AuthService.API.Core.Interfaces
{
    public interface IRoleSevice
    {
        IEnumerable<RoleDTO> GetAllRoles();
    }
}
