using AutoMapper;
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
        private readonly IMapper _mapper;
        public CreateResourceQueryHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork, ICacheService<Resource> cacheService, IMapper mapper)
        {
            _resourceRepository = resourceRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<Resource> Handle(CreateResourceQuery request, CancellationToken cancellationToken)
        {
            var resource = _mapper.Map<Resource>(request.Resource);

            resource.GID = Guid.NewGuid();

            await _resourceRepository.AddAsync(resource);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return resource;
        }
    }
}
