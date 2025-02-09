using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourcesQueries.GetResourceByGid
{
    public class GetResourceByGidQuery : IRequest<Resource>
    {
        public required Guid GID { get; set; }
    }
}
