using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;
using VolunteerService.Persistence.Repositories;

namespace VolunteerService.Persistence.DbContexts
{
    public class UnitOfWork(VolunteersDbContext dbContext) : IUnitOfWork
    {
        private readonly VolunteersDbContext dbContext = dbContext;

        private VolunteerRepository _volunteerRepository;
        private VolunteersDistrictsRepository _volunteersDistrictsRepository;
        private VolunteersGroupsRepository _volunteersGroupsRepository;

        public IRepository<Volunteer> VolunteerRepository => _volunteerRepository ??= new VolunteerRepository(dbContext);
        public IRepository<VolunteersDistricts> VolunteersDistrictsRepository => _volunteersDistrictsRepository ??= new VolunteersDistrictsRepository(dbContext);
        public IRepository<VolunteersGroups> VolunteersGroupsRepository => _volunteersGroupsRepository ??= new VolunteersGroupsRepository(dbContext);
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}