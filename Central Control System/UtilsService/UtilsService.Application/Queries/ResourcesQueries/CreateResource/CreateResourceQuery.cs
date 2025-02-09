using MediatR;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourcesQueries.CreateResource
{
    public class CreateResourceQuery : IRequest<Resource>
    {
        public required Resource Resource { get; set; }
    }
}
