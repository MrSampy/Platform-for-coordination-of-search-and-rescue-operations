using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.ResourcesEventQueries.GetResourcesByEventGID
{
    public class GetResourcesByEventGIDQuery : IRequest<IEnumerable<ResourcesEventDTO>>
    {
        public required Guid EventGID { get; set; }
    }
}
