using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.ResourcesQueries.CreateResource
{
    public class CreateResourceQueryHandler : IRequestHandler<CreateResourceQuery, Resource>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Resource> _cacheService;
        public CreateResourceQueryHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork, ICacheService<Resource> cacheService)
        {
            _resourceRepository = resourceRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<Resource> Handle(CreateResourceQuery request, CancellationToken cancellationToken)
        {
            request.Resource.GID = Guid.NewGuid();

            await _resourceRepository.AddAsync(request.Resource);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return request.Resource;
        }
    }
}
