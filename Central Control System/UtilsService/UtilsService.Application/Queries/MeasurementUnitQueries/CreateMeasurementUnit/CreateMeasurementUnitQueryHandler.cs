using AutoMapper;
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
        private readonly IMapper _mapper;
        public CreateMeasurementUnitQueryHandler(IMeasurementUnitRepository measurementUnitRepository, IUnitOfWork unitOfWork, ICacheService<MeasurementUnit> cacheService, IMapper mapper)
        {
            _measurementUnitRepository = measurementUnitRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<MeasurementUnit> Handle(CreateMeasurementUnitQuery request, CancellationToken cancellationToken)
        {
            var measurementUnit = _mapper.Map<MeasurementUnit>(request.MeasurementUnit);

            measurementUnit.GID = Guid.NewGuid();

            await _measurementUnitRepository.AddAsync(measurementUnit);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return measurementUnit;
        }
    }
}
