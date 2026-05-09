using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.ResourcesQueries.GetResourceByGid
{
    public class GetResourceByGidQueryHandler : IRequestHandler<GetResourceByGidQuery, Resource>
    {
        private readonly IResourceRepository _resourceRepository;

        public GetResourceByGidQueryHandler(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<Resource> Handle(GetResourceByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _resourceRepository.GetByGidAsync(request.GID, cancellationToken);

            return result ?? throw new UtilsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Resource), request.GID.ToString()));
        }
    }
}
