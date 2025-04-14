using AuthService.API.Core.DTO;
using AuthService.API.Core.Interfaces;
using AuthService.API.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Core.Services
{
    public class UserSevice : IUserSevice
    {
        private readonly IConfiguration _configuration;
        protected readonly AuthDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserSevice(AuthDbContext dbContext, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return (from user in _dbContext.Users
                    select new UserDTO
                    {
                        Id = new Guid(user.Id),
                        Name = user.UserName ?? string.Empty,
                        Roles = (from userRole in _dbContext.UserRoles
                                 join role in _dbContext.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == user.Id
                                 select new RoleDTO
                                 {
                                     Id = new Guid(role.Id),
                                     Name = role.Name ?? string.Empty
                                 }).ToList()
                    }).ToList();
        }

        public UserDTO? GetByGID(Guid gid)
        {
            return (from user in _dbContext.Users
                    where user.Id == gid.ToString()
                    select new UserDTO
                    {
                        Id = new Guid(user.Id),
                        Name = user.UserName ?? string.Empty,
                        Roles = (from userRole in _dbContext.UserRoles
                                 join role in _dbContext.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == user.Id
                                 select new RoleDTO
                                 {
                                     Id = new Guid(role.Id),
                                     Name = role.Name ?? string.Empty
                                 }).ToList()
                    }).FirstOrDefault();
        }


        public UserDTO? GetByUserName(string userName)
        {
            return (from user in _dbContext.Users
                    where user.UserName == userName
                    select new UserDTO
                    {
                        Id = new Guid(user.Id),
                        Name = user.UserName ?? string.Empty,
                        Roles = (from userRole in _dbContext.UserRoles
                                 join role in _dbContext.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == user.Id
                                 select new RoleDTO
                                 {
                                     Id = new Guid(role.Id),
                                     Name = role.Name ?? string.Empty
                                 }).ToList()
                    }).FirstOrDefault();

        }
        public UserDTO? GetByEmail(string email)
        {
            return (from user in _dbContext.Users
                    where user.Email == email
                    select new UserDTO
                    {
                        Id = new Guid(user.Id),
                        Name = user.UserName ?? string.Empty,
                        Roles = (from userRole in _dbContext.UserRoles
                                 join role in _dbContext.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == user.Id
                                 select new RoleDTO
                                 {
                                     Id = new Guid(role.Id),
                                     Name = role.Name ?? string.Empty
                                 }).ToList()
                    }).FirstOrDefault();
        }
        public IEnumerable<string> GetAllUserIdsByRole(string roleName)
        {
            return (from user in _dbContext.Users
                    join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                    join role in _dbContext.Roles on userRole.RoleId equals role.Id
                    where role.Name == roleName
                    select user.Id).ToList();
        }

        public async Task<UserDTO> UpdateUserRoles(UserDTO query)
        {
            var selectedRoleIds = query.Roles.Select(r => r.Id.ToString());
            var currentUserRolesIds = _dbContext.UserRoles.AsNoTracking().Where(ur => ur.UserId == query.Id.ToString())
                .ToList().Select(ur => ur.RoleId.ToString());

            foreach (var userRole in currentUserRolesIds.Except(selectedRoleIds))
                _dbContext.UserRoles.Remove(new IdentityUserRole<string>
                {
                    UserId = query.Id.ToString(),
                    RoleId = userRole
                });

            foreach (var roleId in selectedRoleIds.Except(currentUserRolesIds))
                _dbContext.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = query.Id.ToString(),
                    RoleId = roleId
                });

            await _dbContext.SaveChangesAsync();

            return query;
        }
    }
}
