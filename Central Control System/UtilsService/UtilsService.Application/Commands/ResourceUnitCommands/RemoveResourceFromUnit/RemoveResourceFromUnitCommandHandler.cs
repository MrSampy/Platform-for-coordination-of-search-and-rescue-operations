using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Commands.ResourceUnitCommands.RemoveResourceFromUnit
{
    public class RemoveResourceFromUnitCommandHandler : IRequestHandler<RemoveResourceFromUnitCommand>
    {
        private readonly IResourceMeasurementUnitRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<ResourceMeasurementUnit> _cacheService;

        public RemoveResourceFromUnitCommandHandler(IResourceMeasurementUnitRepository repository, IUnitOfWork unitOfWork, ICacheService<ResourceMeasurementUnit> cacheService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(RemoveResourceFromUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(ResourceMeasurementUnit), request.GID.ToString()));
            }

            await _repository.RemoveResourceFromMeasurementUnitAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
