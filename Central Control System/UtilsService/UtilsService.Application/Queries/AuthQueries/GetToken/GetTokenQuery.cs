using MediatR;
using UtilsService.Application.DTOs;

namespace UtilsService.Application.Queries.AuthQueries.GetToken
{
    public class GetTokenQuery : IRequest<TokenInfoDTO>
    {
        public required LoginDTO LoginDTO { get; set; }
    }
}
