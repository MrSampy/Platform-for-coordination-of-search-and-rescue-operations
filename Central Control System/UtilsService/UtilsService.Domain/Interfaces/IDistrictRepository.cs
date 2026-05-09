using UtilsService.Domain.Entities;

namespace UtilsService.Domain.Interfaces
{
    public interface IDistrictRepository
    {
        Task<IEnumerable<District>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null);
        Task<District> GetByGidAsync(Guid gid, CancellationToken cancellationToken);
        Task AddAsync(District entity);
        Task UpdateAsync(District entity);
        Task DeleteAsync(District entity);
    }
}
