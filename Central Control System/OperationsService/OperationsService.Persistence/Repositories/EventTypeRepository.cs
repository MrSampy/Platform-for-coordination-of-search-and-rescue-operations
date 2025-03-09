using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class EventTypeRepository : IRepository<EventType>
    {
        private readonly OperationsDbContext _dbContext;
        public EventTypeRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(EventType entity)
        {
            await _dbContext.EventTypes.AddAsync(entity);
        }

        public Task DeleteAsync(EventType entity)
        {
            _dbContext.EventTypes.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<EventType>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.EventTypes.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<EventType> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.EventTypes.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(EventType entity)
        {
            _dbContext.EventTypes.Update(entity);
            return Task.CompletedTask;
        }
    }
}
