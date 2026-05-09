using MediatR;
using UtilsService.Application.DTOs;
using UtilsService.Application.DTOs.Request;
using UtilsService.Application.DTOs.Response;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.AuthQueries.GetToken
{
    public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, TokenInfoDTO>
    {
        private readonly IApiBuilder _apiBuilder;

        public GetTokenQueryHandler(IApiBuilder apiBuilder)
        {
            _apiBuilder = apiBuilder;
        }

        public async Task<TokenInfoDTO> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            var loginResponse = await _apiBuilder.PostRequest<LoginResponse>("api/authenticate/login", request.LoginDTO, Constants.AuthService, cancellationToken);

            if (loginResponse.IsValid)
            {
                var tokenInfo = await _apiBuilder.PostRequest<TokenInfoDTO>("api/authenticate/gettoken", new GetTokenRequest { Username = request.LoginDTO.Username }, Constants.AuthService, cancellationToken);

                return tokenInfo;
            }
            else
            {
                throw new UtilsServiceException("Invalid credentials");
            }
        }
    }
}
