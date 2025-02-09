using UtilsService.Domain.Entities;

namespace UtilsService.Domain.Interfaces
{
    public interface IResourceRepository
    {
        Task<IEnumerable<Resource>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null);
        Task<Resource> GetByGidAsync(Guid gid, CancellationToken cancellationToken);
        Task AddAsync(Resource entity);
        Task UpdateAsync(Resource entity);
        Task DeleteAsync(Resource entity);
    }
}
