using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.DTOs.Request;
using VolunteerService.Application.DTOs.Response;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.AuthQueries.GetToken
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
                throw new VolunteerServiceException("Invalid credentials");
            }
        }
    }
}
