using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.ResourcesEventQueries.GetByGID
{
    public class GetResourcesEventByGidQuery : IRequest<ResourcesEventDTO>
    {
        public required Guid GID { get; set; }
    }
}
