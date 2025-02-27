using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Commands.ResourceCommands.DeleteResource
{
    public class DeleteResourceCommandHandler : IRequestHandler<DeleteResourceCommand>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IResourceMeasurementUnitRepository _resourceMeasurementUnitRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Resource> _cacheService;

        public DeleteResourceCommandHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork, ICacheService<Resource> cacheService, IResourceMeasurementUnitRepository resourceMeasurementUnitRepository)
        {
            _resourceRepository = resourceRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _resourceMeasurementUnitRepository = resourceMeasurementUnitRepository;
        }

        public async Task Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _resourceRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new UtilsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Resource), request.GID.ToString()));
            }

            await _resourceRepository.DeleteAsync(entity);

            var resourceMeasurementUnits = await _resourceMeasurementUnitRepository.GetMeasurementUnitsByResorceGIDAsync(request.GID, cancellationToken);

            await _resourceMeasurementUnitRepository.DeleteAllMeasurementUnits(resourceMeasurementUnits, cancellationToken);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
