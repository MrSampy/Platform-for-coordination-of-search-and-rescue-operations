using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Queries.ResourcesEventQueries.GetAll
{
    public class GetAllResourcesEventsQuery : IRequest<IEnumerable<ResourcesEventDTO>>
    {
        public required PaginationQuery PaginationQuery { get; set; }
    }
}
