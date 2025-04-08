using MediatR;
using OperationsService.Application.DTOs;

namespace OperationsService.Application.Queries.ResourcesEventQueries.GetEventsByResourceGID
{
    public class GetEventsByResourceGIDQuery : IRequest<IEnumerable<ResourcesEventDTO>>
    {
        public required Guid ResourceGID { get; set; }
    }
}
