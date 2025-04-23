using Microsoft.EntityFrameworkCore;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;
using VolunteerService.Persistence.DbContexts;

namespace VolunteerService.Persistence.Repositories
{
    public class VolunteersEventsRepository : IRepository<VolunteersEvents>
    {
        private readonly VolunteersDbContext _dbContext;

        public VolunteersEventsRepository(VolunteersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(VolunteersEvents entity)
        {
            await _dbContext.VolunteersEvents.AddAsync(entity);
        }

        public Task DeleteAsync(VolunteersEvents entity)
        {
            _dbContext.VolunteersEvents.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<VolunteersEvents>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.VolunteersEvents.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<VolunteersEvents> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.VolunteersEvents.AsNoTracking()
                .FirstOrDefaultAsync(vg => vg.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(VolunteersEvents entity)
        {
            _dbContext.VolunteersEvents.Update(entity);
            return Task.CompletedTask;
        }
    }
}
