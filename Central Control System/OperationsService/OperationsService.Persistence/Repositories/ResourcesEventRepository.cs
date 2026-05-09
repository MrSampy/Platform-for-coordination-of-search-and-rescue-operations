using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class ResourcesEventRepository : IRepository<ResourcesEvent>
    {
        private readonly OperationsDbContext _dbContext;
        public ResourcesEventRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ResourcesEvent entity)
        {
            await _dbContext.ResourcesEvents.AddAsync((ResourcesEvent)entity);
        }

        public Task DeleteAsync(ResourcesEvent entity)
        {
            _dbContext.ResourcesEvents.Remove((ResourcesEvent)entity);
            return Task.CompletedTask;
        }
        public async Task<int> GetTotalCount()
        {
            return await _dbContext.ResourcesEvents.CountAsync();
        }
        public async Task<IEnumerable<ResourcesEvent>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            if (query == null)
            {
                return await _dbContext.ResourcesEvents.AsNoTracking().ToListAsync(cancellationToken);
            }
            else
            {
                return await _dbContext.ResourcesEvents.AsNoTracking()
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync(cancellationToken);
            }
        }

        public async Task<ResourcesEvent> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.ResourcesEvents.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(ResourcesEvent entity)
        {
            _dbContext.ResourcesEvents.Update((ResourcesEvent)entity);
            return Task.CompletedTask;
        }
    }
}
