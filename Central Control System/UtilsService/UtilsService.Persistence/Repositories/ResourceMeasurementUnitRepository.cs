using Microsoft.EntityFrameworkCore;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;
using UtilsService.Persistence.DbContexts;

namespace UtilsService.Persistence.Repositories
{
    public class ResourceMeasurementUnitRepository : IResourceMeasurementUnitRepository
    {
        private readonly UtilsDbContext _dbContexts;
        public ResourceMeasurementUnitRepository(UtilsDbContext dbContexts)
        {
            _dbContexts = dbContexts;
        }

        public async Task AddResourceToMeasurementUnit(ResourceMeasurementUnit entity)
        {
            await _dbContexts.ResourceMeasurementUnits.AddAsync(entity);
        }

        public async Task<ResourceMeasurementUnit> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContexts.ResourceMeasurementUnits.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public async Task<IEnumerable<ResourceMeasurementUnit>> GetMeasurementUnitsByResorceGIDAsync(Guid resourceGid, CancellationToken cancellationToken)
        {
            return await _dbContexts.ResourceMeasurementUnits.AsNoTracking().Where(e => e.ResourceGID == resourceGid).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ResourceMeasurementUnit>> GetResourcesByMeasurementUnitGIDAsync(Guid measurementUnitGid, CancellationToken cancellationToken)
        {
            return await _dbContexts.ResourceMeasurementUnits.AsNoTracking().Where(e => e.UnitGID == measurementUnitGid).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsResourceInMeasurementUnit(ResourceMeasurementUnit entity)
        {
            return (await _dbContexts.ResourceMeasurementUnits
                .Where(x => x.UnitGID == entity.UnitGID && x.ResourceGID == entity.ResourceGID)
                .CountAsync()) == 1;
        }

        public Task RemoveResourceFromMeasurementUnitAsync(ResourceMeasurementUnit entity)
        {
            _dbContexts.ResourceMeasurementUnits.Remove(entity);
            return Task.CompletedTask;
        }
        public async Task DeleteAllMeasurementUnits(IEnumerable<ResourceMeasurementUnit> resourceMeasurementUnits, CancellationToken cancellationToken)
        {
            if (resourceMeasurementUnits != null && resourceMeasurementUnits.Count() > 0)
            {
                foreach (var resourceMeasurementUnit in resourceMeasurementUnits)
                {
                    await RemoveResourceFromMeasurementUnitAsync(resourceMeasurementUnit);
                }
            }
        }
    }
}
