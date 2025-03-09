using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class OperationWorkerRepository : IRepository<OperationWorker>
    {
        private readonly OperationsDbContext _dbContext;
        public OperationWorkerRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(OperationWorker entity)
        {
            await _dbContext.OperationWorkers.AddAsync((OperationWorker)entity);
        }

        public Task DeleteAsync(OperationWorker entity)
        {
            _dbContext.OperationWorkers.Remove((OperationWorker)entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OperationWorker>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            if (query == null)
            {
                return await _dbContext.OperationWorkers.AsNoTracking().ToListAsync(cancellationToken);
            }
            else
            {
                return await _dbContext.OperationWorkers.AsNoTracking()
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync(cancellationToken);
            }
        }

        public async Task<OperationWorker> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.OperationWorkers.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(OperationWorker entity)
        {
            _dbContext.OperationWorkers.Update((OperationWorker)entity);
            return Task.CompletedTask;
        }
    }
}
