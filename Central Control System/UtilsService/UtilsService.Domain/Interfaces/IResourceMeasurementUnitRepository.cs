using UtilsService.Domain.Entities;

namespace UtilsService.Domain.Interfaces
{
    public interface IResourceMeasurementUnitRepository
    {
        Task<ResourceMeasurementUnit> GetByGidAsync(Guid gid, CancellationToken cancellationToken);
        Task<IEnumerable<ResourceMeasurementUnit>> GetMeasurementUnitsByResorceGIDAsync(Guid resourceGid, CancellationToken cancellationToken);
        Task<IEnumerable<ResourceMeasurementUnit>> GetResourcesByMeasurementUnitGIDAsync(Guid measurementUnitGid, CancellationToken cancellationToken);
        Task<bool> IsResourceInMeasurementUnit(ResourceMeasurementUnit entity);
        Task AddResourceToMeasurementUnit(ResourceMeasurementUnit entity);
        Task RemoveResourceFromMeasurementUnitAsync(ResourceMeasurementUnit entity);
        Task DeleteAllMeasurementUnits(IEnumerable<ResourceMeasurementUnit> resourceMeasurementUnits, CancellationToken cancellationToken);
    }
}
