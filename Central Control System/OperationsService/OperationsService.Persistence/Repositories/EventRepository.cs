using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class EventRepository : IRepository<Event>
    {
        private readonly OperationsDbContext _dbContext;
        public EventRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Event entity)
        {
            await _dbContext.Events.AddAsync(entity);
        }

        public Task DeleteAsync(Event entity)
        {
            _dbContext.Events.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.Events.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<Event> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.Events.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(Event entity)
        {
            _dbContext.Events.Update(entity);
            return Task.CompletedTask;
        }
    }
}
