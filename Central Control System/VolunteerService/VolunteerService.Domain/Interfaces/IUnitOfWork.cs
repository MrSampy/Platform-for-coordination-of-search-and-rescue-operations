using VolunteerService.Domain.Entities;

namespace VolunteerService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Volunteer> VolunteerRepository { get; }
        IRepository<VolunteersDistricts> VolunteersDistrictsRepository { get; }
        IRepository<VolunteersGroups> VolunteersGroupsRepository { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
