using Microsoft.EntityFrameworkCore;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;
using UtilsService.Persistence.DbContexts;

namespace UtilsService.Persistence.Repositories
{
    public class MeasurementUnitRepository : IMeasurementUnitRepository
    {
        private readonly UtilsDbContext _dbContexts;

        public MeasurementUnitRepository(UtilsDbContext dbContexts)
        {
            _dbContexts = dbContexts;
        }

        public async Task AddAsync(MeasurementUnit entity)
        {
            await _dbContexts.MeasurementUnits.AddAsync(entity);
        }

        public Task DeleteAsync(MeasurementUnit entity)
        {
            _dbContexts.MeasurementUnits.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<MeasurementUnit>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            if (query == null)
            {
                return await _dbContexts.MeasurementUnits
                .AsNoTracking().ToListAsync(cancellationToken);
            }
            else
            {
                return await _dbContexts.MeasurementUnits
                .AsNoTracking()
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);
            }

        }

        public async Task<MeasurementUnit> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContexts.MeasurementUnits.AsNoTracking().FirstOrDefaultAsync(e => e.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(MeasurementUnit entity)
        {
            _dbContexts.MeasurementUnits.Update(entity);
            return Task.CompletedTask;
        }
    }
}
