using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
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
            return await _apiBuilder.PostRequest<TokenInfoDTO>("api/authenticate/login", request.LoginDTO, Constants.AuthService, cancellationToken);
        }
    }
}
