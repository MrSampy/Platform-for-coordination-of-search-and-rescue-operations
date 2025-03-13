using Microsoft.EntityFrameworkCore;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;
using VolunteerService.Persistence.DbContexts;

namespace VolunteerService.Persistence.Repositories
{
    public class VolunteersDistrictsRepository : IRepository<VolunteersDistricts>
    {
        private readonly VolunteersDbContext _dbContext;

        public VolunteersDistrictsRepository(VolunteersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(VolunteersDistricts entity)
        {
            await _dbContext.VolunteersDistricts.AddAsync(entity);
        }

        public Task DeleteAsync(VolunteersDistricts entity)
        {
            _dbContext.VolunteersDistricts.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<VolunteersDistricts>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.VolunteersDistricts.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<VolunteersDistricts> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.VolunteersDistricts.AsNoTracking()
                .FirstOrDefaultAsync(vd => vd.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(VolunteersDistricts entity)
        {
            _dbContext.VolunteersDistricts.Update(entity);
            return Task.CompletedTask;
        }
    }

}
