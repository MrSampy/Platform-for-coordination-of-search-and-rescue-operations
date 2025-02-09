using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.MeasurementUnitQueries.CreateMeasurementUnit
{
    public class CreateMeasurementUnitQueryHandler : IRequestHandler<CreateMeasurementUnitQuery, MeasurementUnit>
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<MeasurementUnit> _cacheService;
        public CreateMeasurementUnitQueryHandler(IMeasurementUnitRepository measurementUnitRepository, IUnitOfWork unitOfWork, ICacheService<MeasurementUnit> cacheService)
        {
            _measurementUnitRepository = measurementUnitRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<MeasurementUnit> Handle(CreateMeasurementUnitQuery request, CancellationToken cancellationToken)
        {
            request.MeasurementUnit.GID = Guid.NewGuid();

            await _measurementUnitRepository.AddAsync(request.MeasurementUnit);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return request.MeasurementUnit;
        }
    }
}
