using AuthService.API.Core.DTO;
using AuthService.API.Core.Interfaces;
using AuthService.API.Core.Models;
using AuthService.API.Exceptions;
using Microsoft.AspNetCore.Identity;
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

        public async Task<string> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null) throw new KeyNotFoundException("User already exists!");

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            if (!await _roleManager.RoleExistsAsync(UserRoles.Volunteer))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Volunteer));
            }

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

            return savedUser.Id;
        }

        public async Task RegisterAdmin(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                throw new AuthServiceException("User already exists!");
            }

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

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Volunteer))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Volunteer));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.OperationWorker))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.OperationWorker));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRolesAsync(user, new List<string> { UserRoles.Admin, UserRoles.Volunteer });
            }
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
    }
}
