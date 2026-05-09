using AuthService.API.Core.DTO;
using AuthService.API.Core.Interfaces;
using AuthService.API.Infrastructure;

namespace AuthService.API.Core.Services
{
    public class RoleSevice : IRoleSevice
    {
        private readonly IConfiguration _configuration;
        protected readonly AuthDbContext _dbContext;

        public RoleSevice(AuthDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            return (from role in _dbContext.Roles
                    select new RoleDTO
                    {
                        Id = new Guid(role.Id),
                        Name = role.Name ?? string.Empty
                    }).ToList();
        }
    }
}
