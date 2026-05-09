using VolunteerService.Domain.Entities;

namespace VolunteerService.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null);
        Task<T> GetByGidAsync(Guid gid, CancellationToken cancellationToken);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
