using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Commands.MeasurementUnitCommands.UpdateMeasurementUnit
{
    public class UpdateMeasurementUnitCommandHandler : IRequestHandler<UpdateMeasurementUnitCommand>
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<MeasurementUnit> _cacheService;

        public UpdateMeasurementUnitCommandHandler(IMeasurementUnitRepository measurementUnitRepository, IUnitOfWork unitOfWork, ICacheService<MeasurementUnit> cacheService)
        {
            _measurementUnitRepository = measurementUnitRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(UpdateMeasurementUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _measurementUnitRepository.GetByGidAsync(request.MeasurementUnit.GID, cancellationToken);

            if (entity == null)
            {
                throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(MeasurementUnit), request.MeasurementUnit.GID.ToString()));
            }

            await _measurementUnitRepository.UpdateAsync(request.MeasurementUnit);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
