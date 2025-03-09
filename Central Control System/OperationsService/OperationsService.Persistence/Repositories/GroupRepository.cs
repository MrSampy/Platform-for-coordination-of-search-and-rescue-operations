using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.Persistence.Repositories
{
    public class GroupRepository : IRepository<Group>
    {
        private readonly OperationsDbContext _dbContext;
        public GroupRepository(OperationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Group entity)
        {
            await _dbContext.Groups.AddAsync(entity);
        }

        public Task DeleteAsync(Group entity)
        {
            _dbContext.Groups.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.Groups.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<Group> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(Group entity)
        {
            _dbContext.Groups.Update(entity);
            return Task.CompletedTask;
        }
    }
}
