using Microsoft.EntityFrameworkCore;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;
using UtilsService.Persistence.DbContexts;

namespace UtilsService.Persistence.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly UtilsDbContext _dbContexts;

        public ResourceRepository(UtilsDbContext dbContexts)
        {
            _dbContexts = dbContexts;
        }

        public async Task AddAsync(Resource entity)
        {
            await _dbContexts.Resources.AddAsync(entity);
        }

        public Task DeleteAsync(Resource entity)
        {
            _dbContexts.Resources.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Resource>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            if (query == null)
            {
                return await _dbContexts.Resources
                .AsNoTracking().ToListAsync(cancellationToken);
            }
            else
            {
                return await _dbContexts.Resources
                .AsNoTracking()
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);
            }

        }

        public async Task<Resource> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContexts.Resources.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(Resource entity)
        {
            _dbContexts.Resources.Update(entity);
            return Task.CompletedTask;
        }
    }
}
