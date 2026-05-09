using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Commands.MeasurementUnitCommands.DeleteMeasurementUnit
{
    public class DeleteMeasurementUnitCommandHandler : IRequestHandler<DeleteMeasurementUnitCommand>
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;
        private readonly IResourceMeasurementUnitRepository _resourceMeasurementUnitRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<MeasurementUnit> _cacheService;
        public DeleteMeasurementUnitCommandHandler(IMeasurementUnitRepository measurementUnitRepository, IUnitOfWork unitOfWork, ICacheService<MeasurementUnit> cacheService, IResourceMeasurementUnitRepository resourceMeasurementUnitRepository)
        {
            _measurementUnitRepository = measurementUnitRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _resourceMeasurementUnitRepository = resourceMeasurementUnitRepository;
        }

        public async Task Handle(DeleteMeasurementUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _measurementUnitRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new UtilsServiceException(string.Format(Constants.NotFoundEntityException, nameof(MeasurementUnit), request.GID.ToString()));
            }

            await _measurementUnitRepository.DeleteAsync(entity);

            var resourceMeasurementUnits = await _resourceMeasurementUnitRepository.GetResourcesByMeasurementUnitGIDAsync(request.GID, cancellationToken);

            await _resourceMeasurementUnitRepository.DeleteAllMeasurementUnits(resourceMeasurementUnits, cancellationToken);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
