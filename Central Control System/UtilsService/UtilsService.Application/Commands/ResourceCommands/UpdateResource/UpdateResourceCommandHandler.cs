using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Commands.ResourceCommands.UpdateResource
{
    public class UpdateResourceCommandHandler : IRequestHandler<UpdateResourceCommand>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Resource> _cacheService;

        public UpdateResourceCommandHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork, ICacheService<Resource> cacheService)
        {
            _resourceRepository = resourceRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(UpdateResourceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _resourceRepository.GetByGidAsync(request.Resource.GID, cancellationToken);

            if (entity == null)
            {
                throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(Resource), request.Resource.GID.ToString()));
            }

            await _resourceRepository.UpdateAsync(request.Resource);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
