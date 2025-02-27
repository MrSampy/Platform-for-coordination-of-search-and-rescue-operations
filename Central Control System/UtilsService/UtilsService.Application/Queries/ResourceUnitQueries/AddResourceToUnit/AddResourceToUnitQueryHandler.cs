using AutoMapper;
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
        private readonly IMapper _mapper;

        public AddResourceToUnitQueryHandler(IResourceMeasurementUnitRepository repository, IUnitOfWork unitOfWork, ICacheService<ResourceMeasurementUnit> cacheService,
            IResourceRepository resourceRepository, IMeasurementUnitRepository unitRepository, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _resourceRepository = resourceRepository;
            _unitRepository = unitRepository;
            _mapper = mapper;
        }

        public async Task<ResourceMeasurementUnit> Handle(AddResourceToUnitQuery request, CancellationToken cancellationToken)
        {
            var resourceMeasurementUnit = _mapper.Map<ResourceMeasurementUnit>(request.ResourceMeasurementUnit);

            if (await _repository.IsResourceInMeasurementUnit(resourceMeasurementUnit))
            {
                throw new UtilsServiceException(Constants.AlreadyExistsSuchCombinationOfResourceUnitException);
            }

            var resource = await _resourceRepository.GetByGidAsync(request.ResourceMeasurementUnit.ResourceGID, cancellationToken);

            if (resource == null)
            {
                throw new UtilsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Resource), request.ResourceMeasurementUnit.ResourceGID.ToString()));
            }

            var unit = await _unitRepository.GetByGidAsync(request.ResourceMeasurementUnit.UnitGID, cancellationToken);

            if (unit == null)
            {
                throw new UtilsServiceException(string.Format(Constants.NotFoundEntityException, nameof(MeasurementUnit), request.ResourceMeasurementUnit.UnitGID.ToString()));
            }


            resourceMeasurementUnit.GID = Guid.NewGuid();

            await _repository.AddResourceToMeasurementUnit(resourceMeasurementUnit);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return resourceMeasurementUnit;
        }
    }
}
