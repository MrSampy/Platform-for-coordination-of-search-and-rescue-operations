using Gateway.DTO.DTOs.Volunteers;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IVolunteersService
    {
        Task<VolunteerDTO?> GeVolunteerByUserGID(Guid userGID, CancellationToken cancellationToken, string token);
    }
}
