using Gateway.DTO.DTOs.Volunteers;
using Gateway.DTO.DTOs.Volunteers.Request;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IVolunteersService
    {
        Task<VolunteerDTO?> GeVolunteerByUserGID(Guid userGID, CancellationToken cancellationToken, string token);
        Task<IEnumerable<VolunteerDTO>> GetVolunteersForEvent(VolunteersForEventRequest request, CancellationToken cancellationToken, string token);
    }
}
