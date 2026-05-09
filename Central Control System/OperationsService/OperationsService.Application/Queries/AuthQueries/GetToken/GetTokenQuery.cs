
using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.AuthQueries.GetToken
{
    public class GetTokenQuery : IRequest<TokenInfoDTO>
    {
        public required LoginDTO LoginDTO { get; set; }
    }
}
