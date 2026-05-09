using Microsoft.EntityFrameworkCore;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;
using VolunteerService.Persistence.DbContexts;

namespace VolunteerService.Persistence.Repositories
{
    public class VolunteersGroupsRepository : IRepository<VolunteersGroups>
    {
        private readonly VolunteersDbContext _dbContext;

        public VolunteersGroupsRepository(VolunteersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(VolunteersGroups entity)
        {
            await _dbContext.VolunteersGroups.AddAsync(entity);
        }

        public Task DeleteAsync(VolunteersGroups entity)
        {
            _dbContext.VolunteersGroups.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<VolunteersGroups>> GetAllAsync(CancellationToken cancellationToken, PaginationQuery query = null)
        {
            var queryable = _dbContext.VolunteersGroups.AsNoTracking();
            if (query != null)
            {
                queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            }
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<VolunteersGroups> GetByGidAsync(Guid gid, CancellationToken cancellationToken)
        {
            return await _dbContext.VolunteersGroups.AsNoTracking()
                .FirstOrDefaultAsync(vg => vg.GID == gid, cancellationToken);
        }

        public Task UpdateAsync(VolunteersGroups entity)
        {
            _dbContext.VolunteersGroups.Update(entity);
            return Task.CompletedTask;
        }
    }

}
