using Microsoft.EntityFrameworkCore;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;
using VolunteerService.Persistence.DbContexts;

namespace VolunteerService.Persistence.Repositories
{
    public class VolunteerRepository : IRepository<Volunteer>
    {
        private readonly VolunteersDbContext _dbContext;

        public VolunteerRepository(VolunteersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Volunteer entity)
        {
            await _dbContext.Volunteers.AddAsync(entity);
        }

        public Task DeleteAsync(Volunteer entity)
        {
            _dbContext.Volunteers.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Volunteer>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.Volunteers.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<Volunteer> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.Volunteers.AsNoTracking()
                .FirstOrDefaultAsync(v => v.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(Volunteer entity)
        {
            _dbContext.Volunteers.Update(entity);
            return Task.CompletedTask;
        }
    }

}
