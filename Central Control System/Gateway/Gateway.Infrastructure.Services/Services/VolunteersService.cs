using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Volunteers;

namespace Gateway.Infrastructure.Services.Services
{
    public class VolunteersService : IVolunteersService
    {
        private IVolunteersGateway _volunteersGateway;
        public VolunteersService(IVolunteersGateway volunteersGateway)
        {
            _volunteersGateway = volunteersGateway;
        }

        public async Task<VolunteerDTO?> GeVolunteerByUserGID(Guid userGID, CancellationToken cancellationToken, string token)
        {
            var volunteers = await _volunteersGateway.GetVolunteers(new DTO.DTOs.Common.PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);
            return volunteers.FirstOrDefault(x => x.UserGID == userGID);
        }
    }
}
