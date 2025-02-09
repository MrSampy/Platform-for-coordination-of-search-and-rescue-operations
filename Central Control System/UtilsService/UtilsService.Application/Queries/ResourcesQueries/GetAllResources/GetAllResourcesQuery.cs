using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourcesQueries.GetAllResources
{
    public class GetAllResourcesQuery : IRequest<IEnumerable<Resource>>
    {
        public PaginationQuery PaginationQuery { get; set; }
    }
}
