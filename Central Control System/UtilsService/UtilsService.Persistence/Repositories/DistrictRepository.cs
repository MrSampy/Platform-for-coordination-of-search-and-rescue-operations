using Microsoft.EntityFrameworkCore;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;
using UtilsService.Persistence.DbContexts;

namespace UtilsService.Persistence.Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly UtilsDbContext _dbContexts;
        public DistrictRepository(UtilsDbContext dbContexts)
        {
            _dbContexts = dbContexts;
        }

        public async Task AddAsync(District entity)
        {
            await _dbContexts.Districts.AddAsync(entity);
        }

        public Task DeleteAsync(District entity)
        {
            _dbContexts.Districts.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<District>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            if (query == null)
            {
                return await _dbContexts.Districts
                .AsNoTracking().ToListAsync(cancellationToken);
            }
            else
            {
                return await _dbContexts.Districts
                .AsNoTracking()
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);
            }

        }

        public async Task<District> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContexts.Districts.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(District entity)
        {
            _dbContexts.Districts.Update(entity);
            return Task.CompletedTask;
        }
    }
}
