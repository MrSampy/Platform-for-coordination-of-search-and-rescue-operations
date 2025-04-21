using AuthService.API.Core.DTO;
using AuthService.API.Core.Interfaces;
using AuthService.API.Core.Models;
using AuthService.API.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AuthService.API.Core.Services
{
    public class AuthenticateSevice : IAuthenticateSevice
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticateSevice(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<MeResponse> Me(HttpContext httpContext)
        {
            var result = new MeResponse() { IsValid = false, User = null };

            if (httpContext == null || httpContext.User == null || httpContext.User.Claims == null)
            {
                return result;
            }

            var claims = httpContext.User.Claims.ToList();

            var userIdClaim = claims.FirstOrDefault(c => c.Type == "UserIdentifier");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return result;
            }

            var userId = userIdClaim.Value;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return result;
            }

            result.IsValid = true;

            result.User = await GetUserFromIdentity(user);

            return result;
        }

        public async Task<TokenInfoDTO> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new UnauthorizedAccessException("Wrong user credentials!");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
        {
            new("UserIdentifier", user.Id),
            new("UserName", model.Username),
            new("Jti", Guid.NewGuid().ToString())
        };

            if (userRoles.Any())
                authClaims.Add(new Claim("Roles", JsonSerializer.Serialize(userRoles.ToList()),
                    JsonClaimValueTypes.JsonArray));

            var token = GetToken(authClaims);

            return new TokenInfoDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }

        public async Task<UserDTO> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null) throw new KeyNotFoundException("User already exists!");

            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null) throw new KeyNotFoundException("User with such email already exists!");

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            await CreateRoles();

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new AuthServiceException("User creation failed! Please check user details and try again.");
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Volunteer))
            {
                await _userManager.AddToRolesAsync(user, new List<string> { UserRoles.Volunteer });
            }

            var savedUser = await _userManager.FindByNameAsync(model.Username);

            if (savedUser == null)
            {
                throw new AuthServiceException("User creation failed! Please check user details and try again.");
            }

            return await GetUserFromIdentity(savedUser);
        }

        private async Task CreateRoles()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Volunteer))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Volunteer));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Dispatcher))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Dispatcher));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Coordinator))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Coordinator));
            }
        }
        public async Task<UserDTO> RegisterCoordinator(RegisterModel model)
        {
            return await RegisterWithRoles(model, new List<string> { UserRoles.Coordinator });
        }

        public async Task<UserDTO> RegisterDispatcher(RegisterModel model)
        {
            return await RegisterWithRoles(model, new List<string> { UserRoles.Dispatcher });
        }
        public async Task<UserDTO> RegisterAdmin(RegisterModel model)
        {
            return await RegisterWithRoles(model, new List<string> { UserRoles.Admin, UserRoles.Dispatcher });
        }

        private async Task<UserDTO> RegisterWithRoles(RegisterModel model, List<string> roles)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                throw new AuthServiceException("User already exists!");
            }

            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null) throw new KeyNotFoundException("User with such email already exists!");

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new AuthServiceException("User creation failed! Please check user details and try again.");
            }

            await CreateRoles();

            var validRoles = new List<string>();

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    validRoles.Add(role);
                }
            }

            if (validRoles.Any())
            {
                await _userManager.AddToRolesAsync(user, validRoles);
            }

            var savedUser = await _userManager.FindByNameAsync(model.Username);

            if (savedUser == null)
            {
                throw new AuthServiceException("User creation failed! Please check user details and try again.");
            }

            return await GetUserFromIdentity(savedUser);
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"] ?? string.Empty));

            var token = new JwtSecurityToken(
                _configuration["ValidIssuer"],
                _configuration["ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        private async Task<UserDTO> GetUserFromIdentity(IdentityUser user)
        {
            var roleNames = await _userManager.GetRolesAsync(user);

            var roles = new List<RoleDTO>();

            if (roleNames != null)
            {
                foreach (var roleName in roleNames)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);

                    if (role != null)
                    {
                        roles.Add(new RoleDTO
                        {
                            Id = Guid.Parse(role.Id),
                            Name = role.Name!
                        });
                    }
                }
            }

            return new UserDTO { Id = new Guid(user.Id), Name = user.UserName!, Roles = roles, Email = user.Email! };
        }
    }
}
