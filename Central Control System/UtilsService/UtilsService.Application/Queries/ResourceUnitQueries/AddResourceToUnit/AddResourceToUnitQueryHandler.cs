using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;
namespace UtilsService.Application.Queries.ResourceUnitQueries.AddResourceToUnit
{
    public class AddResourceToUnitQueryHandler : IRequestHandler<AddResourceToUnitQuery, ResourceMeasurementUnit>
    {
        private readonly IResourceMeasurementUnitRepository _repository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IMeasurementUnitRepository _unitRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<ResourceMeasurementUnit> _cacheService;

        public AddResourceToUnitQueryHandler(IResourceMeasurementUnitRepository repository, IUnitOfWork unitOfWork, ICacheService<ResourceMeasurementUnit> cacheService, IResourceRepository resourceRepository, IMeasurementUnitRepository unitRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _resourceRepository = resourceRepository;
            _unitRepository = unitRepository;
        }

        public async Task<ResourceMeasurementUnit> Handle(AddResourceToUnitQuery request, CancellationToken cancellationToken)
        {
            if (await _repository.IsResourceInMeasurementUnit(request.ResourceMeasurementUnit))
            {
                throw new CustomException(Constants.AlreadyExistsSuchCombinationOfResourceUnitException);
            }

            var resource = await _resourceRepository.GetByGidAsync(request.ResourceMeasurementUnit.ResourceGID, cancellationToken);

            if (resource == null)
            {
                throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(Resource), request.ResourceMeasurementUnit.ResourceGID.ToString()));
            }

            var unit = await _unitRepository.GetByGidAsync(request.ResourceMeasurementUnit.UnitGID, cancellationToken);

            if (unit == null)
            {
                throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(MeasurementUnit), request.ResourceMeasurementUnit.UnitGID.ToString()));
            }

            request.ResourceMeasurementUnit.GID = Guid.NewGuid();

            await _repository.AddResourceToMeasurementUnit(request.ResourceMeasurementUnit);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return request.ResourceMeasurementUnit;
        }
    }
}
