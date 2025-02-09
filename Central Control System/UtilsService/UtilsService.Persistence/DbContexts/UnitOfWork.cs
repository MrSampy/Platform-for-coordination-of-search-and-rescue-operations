using UtilsService.Domain.Interfaces;
using UtilsService.Persistence.Repositories;

namespace UtilsService.Persistence.DbContexts
{
    public sealed class UnitOfWork(UtilsDbContext dbContext) : IUnitOfWork
    {
        private readonly UtilsDbContext dbContext = dbContext;

        private DistrictRepository _districtRepository;
        private MeasurementUnitRepository _measurementUnitRepository;
        private ResourceRepository _resourceRepository;
        private ResourceMeasurementUnitRepository _resourceMeasurementUnitRepository;
        public IDistrictRepository DistrictRepository => _districtRepository ??= new DistrictRepository(dbContext);
        public IMeasurementUnitRepository MeasurementUnitRepository => _measurementUnitRepository ??= new MeasurementUnitRepository(dbContext);
        public IResourceRepository ResourceRepository => _resourceRepository ??= new ResourceRepository(dbContext);
        public IResourceMeasurementUnitRepository ResourceMeasurementUnitRepository => _resourceMeasurementUnitRepository ??= new ResourceMeasurementUnitRepository(dbContext);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

}
