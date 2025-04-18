using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class OperationTaskStatusRepository : IRepository<OperationTaskStatus>
    {
        private readonly OperationsDbContext _dbContext;
        public OperationTaskStatusRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(OperationTaskStatus entity)
        {
            await _dbContext.OperationTaskStatuses.AddAsync((OperationTaskStatus)entity);
        }

        public Task DeleteAsync(OperationTaskStatus entity)
        {
            _dbContext.OperationTaskStatuses.Remove((OperationTaskStatus)entity);
            return Task.CompletedTask;
        }
        public async Task<int> GetTotalCount()
        {
            return await _dbContext.OperationTaskStatuses.CountAsync();
        }
        public async Task<IEnumerable<OperationTaskStatus>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            if (query == null)
            {
                return await _dbContext.OperationTaskStatuses.AsNoTracking().ToListAsync(cancellationToken);
            }
            else
            {
                return await _dbContext.OperationTaskStatuses.AsNoTracking()
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync(cancellationToken);
            }
        }

        public async Task<OperationTaskStatus> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.OperationTaskStatuses.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(OperationTaskStatus entity)
        {
            _dbContext.OperationTaskStatuses.Update((OperationTaskStatus)entity);
            return Task.CompletedTask;
        }
    }

}
