using UtilsService.Domain.Entities;

namespace UtilsService.Domain.Interfaces
{
    public interface IMeasurementUnitRepository
    {
        Task<IEnumerable<MeasurementUnit>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null);
        Task<MeasurementUnit> GetByGidAsync(Guid gid, CancellationToken cancellationToken);
        Task AddAsync(MeasurementUnit entity);
        Task UpdateAsync(MeasurementUnit entity);
        Task DeleteAsync(MeasurementUnit entity);
    }
}
