using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;

namespace OperationsService.Application.Queries.ResourcesEventQueries.Create
{
    public class CreateResourcesEventQuery : IRequest<ResourcesEventDTO>
    {
        public required CreateResourcesEventDTO ResourcesEvent { get; set; }
        public string Token { get; set; }
    }
}
