using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class MessageRepository : IRepository<Message>
    {
        private readonly OperationsDbContext _dbContext;
        public MessageRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Message entity)
        {
            await _dbContext.Messages.AddAsync(entity);
        }

        public Task DeleteAsync(Message entity)
        {
            _dbContext.Messages.Remove(entity);
            return Task.CompletedTask;
        }
        public async Task<int> GetTotalCount()
        {
            return await _dbContext.Messages.CountAsync();
        }
        public async Task<IEnumerable<Message>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.Messages.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<Message> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.Messages.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(Message entity)
        {
            _dbContext.Messages.Update(entity);
            return Task.CompletedTask;
        }
    }
}
