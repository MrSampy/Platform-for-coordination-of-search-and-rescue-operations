using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class EventStatusRepository : IRepository<EventStatus>
    {
        private readonly OperationsDbContext _dbContext;
        public EventStatusRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(EventStatus entity)
        {
            await _dbContext.EventStatuses.AddAsync(entity);
        }

        public Task DeleteAsync(EventStatus entity)
        {
            _dbContext.EventStatuses.Remove(entity);
            return Task.CompletedTask;
        }
        public async Task<int> GetTotalCount()
        {
            return await _dbContext.EventStatuses.CountAsync();
        }
        public async Task<IEnumerable<EventStatus>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.EventStatuses.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<EventStatus> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.EventStatuses.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(EventStatus entity)
        {
            _dbContext.EventStatuses.Update(entity);
            return Task.CompletedTask;
        }
    }
}
