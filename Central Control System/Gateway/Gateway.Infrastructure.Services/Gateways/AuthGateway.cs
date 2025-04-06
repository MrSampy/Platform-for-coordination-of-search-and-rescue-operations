using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Auth;
using Gateway.DTO.Exceptions;

namespace Gateway.Infrastructure.Services.Gateways
{
    public class AuthGateway : IAuthGateway
    {
        private readonly IApiBuilder _apiBuilder;

        public AuthGateway(IApiBuilder apiBuilder)
        {
            _apiBuilder = apiBuilder;
        }

        public async Task<UserDTO> Register(RegisterModel model)
        {
            var response = await _apiBuilder.PostRequest<UserDTO>(
                "api/authenticate/register",
                model,
                SharedConstants.AuthService,
                CancellationToken.None);

            return response;
        }

        public async Task<UserDTO> RegisterAdmin(RegisterModel model, string token)
        {
            var response = await _apiBuilder.PostRequest<UserDTO>(
                "api/authenticate/register-admin",
                model,
                SharedConstants.AuthService,
                CancellationToken.None,
                token);

            return response;
        }

        public async Task<TokenInfoDTO> Login(LoginModel model)
        {
            return await _apiBuilder.PostRequest<TokenInfoDTO>(
                "api/authenticate/login",
                model,
                SharedConstants.AuthService,
                CancellationToken.None);
        }

        public async Task<UserDTO> RegisterCoordinator(RegisterModel model, string token)
        {
            var response = await _apiBuilder.PostRequest<UserDTO>(
                "api/authenticate/register-coordinator",
                model,
                SharedConstants.AuthService,
                CancellationToken.None,
                token);
            return response;
        }

        public async Task<UserDTO> RegisterDispatcher(RegisterModel model, string token)
        {
            var response = await _apiBuilder.PostRequest<UserDTO>(
                "api/authenticate/register-dispatcher",
                model,
                SharedConstants.AuthService,
                CancellationToken.None,
                token);
            return response;
        }

        public IEnumerable<RoleDTO> GetAllRoles(CancellationToken cancellationToken, string token)
        {
            return _apiBuilder
                .GetRequest<IEnumerable<RoleDTO>>(
                    "api/role/collection",
                    SharedConstants.AuthService,
                    cancellationToken,
                    token)
                .GetAwaiter()
                .GetResult();
        }

        public IEnumerable<UserDTO> GetAllUsers(CancellationToken cancellationToken, string token)
        {
            return _apiBuilder
                .GetRequest<IEnumerable<UserDTO>>(
                    "api/user/collection",
                    SharedConstants.AuthService,
                    cancellationToken,
                    token)
                .GetAwaiter()
                .GetResult();
        }

        public UserDTO? GetByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return _apiBuilder
                .GetRequest<UserDTO>(
                    $"api/user/bygid/{gid}",
                    SharedConstants.AuthService,
                    cancellationToken,
                    token)
                .GetAwaiter()
                .GetResult();
        }

        public IEnumerable<string> GetAllUserIdsByRole(string roleName, CancellationToken cancellationToken, string token)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ServiceException(string.Format(SharedConstants.InvalidFieldFormatException, nameof(roleName)));
            }

            return _apiBuilder
                .GetRequest<IEnumerable<string>>(
                    $"api/user/{roleName}",
                    SharedConstants.AuthService,
                    cancellationToken,
                    token)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<UserDTO> UpdateUserRoles(UserDTO query, string token)
        {
            return await _apiBuilder.PutRequest<UserDTO>(
                "api/user",
                query,
                SharedConstants.AuthService,
                CancellationToken.None,
                token);
        }
    }
}
