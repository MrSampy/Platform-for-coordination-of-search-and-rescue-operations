using MediatR;
using VolunteerService.Application.DTOs;

namespace VolunteerService.Application.Queries.AuthQueries.GetToken
{
    public class GetTokenQuery : IRequest<TokenInfoDTO>
    {
        public required LoginDTO LoginDTO { get; set; }
    }
}
