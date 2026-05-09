using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class OperationTaskRepository : IRepository<OperationTask>
    {
        private readonly OperationsDbContext _dbContext;
        public OperationTaskRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(OperationTask entity)
        {
            await _dbContext.OperationTasks.AddAsync((OperationTask)entity);
        }

        public Task DeleteAsync(OperationTask entity)
        {
            _dbContext.OperationTasks.Remove((OperationTask)entity);
            return Task.CompletedTask;
        }
        public async Task<int> GetTotalCount()
        {
            return await _dbContext.OperationTasks.CountAsync();
        }
        public async Task<IEnumerable<OperationTask>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            if (query == null)
            {
                return await _dbContext.OperationTasks.AsNoTracking().ToListAsync(cancellationToken);
            }
            else
            {
                return await _dbContext.OperationTasks.AsNoTracking()
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync(cancellationToken);
            }
        }

        public async Task<OperationTask> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.OperationTasks.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(OperationTask entity)
        {
            _dbContext.OperationTasks.Update(entity);
            return Task.CompletedTask;
        }
    }
}
