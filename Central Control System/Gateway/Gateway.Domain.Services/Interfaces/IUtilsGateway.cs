using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Utils;
using Gateway.DTO.DTOs.Utils.Create;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IUtilsGateway
    {
        Task<IEnumerable<DistrictDTO>> GetDistricts(PaginationQuery paginationQuery, CancellationToken cancellationToken);
        Task<DistrictDTO> GetDistrictByGID(Guid gid, CancellationToken cancellationToken);
        Task<DistrictDTO> CreateDistrict(CreateDistrictDTO district);
        Task UpdateDistrict(DistrictDTO district);
        Task DeleteDistrict(Guid gid);
    }
}
