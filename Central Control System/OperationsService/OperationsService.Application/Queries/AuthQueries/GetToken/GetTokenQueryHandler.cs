using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Requests;
using OperationsService.Application.DTOs.Responses;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.AuthQueries.GetToken
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
                throw new OperationsServiceException("Invalid credentials");
            }
        }
    }
}
