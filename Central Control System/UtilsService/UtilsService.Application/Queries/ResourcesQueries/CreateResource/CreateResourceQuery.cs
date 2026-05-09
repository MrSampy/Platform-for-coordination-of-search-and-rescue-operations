using MediatR;
using UtilsService.Application.DTOs;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Queries.ResourcesQueries.CreateResource
{
    public class CreateResourceQuery : IRequest<Resource>
    {
        public required CreateResourceDTO Resource { get; set; }
    }
}
