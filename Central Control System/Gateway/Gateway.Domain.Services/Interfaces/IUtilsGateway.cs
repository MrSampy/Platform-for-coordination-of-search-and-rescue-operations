using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Utils;
using Gateway.DTO.DTOs.Utils.Create;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IUtilsGateway
    {
        #region District
        Task<IEnumerable<DistrictDTO>> GetDistricts(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<DistrictDTO> GetDistrictByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<DistrictDTO> CreateDistrict(CreateDistrictDTO district, string token);
        Task UpdateDistrict(DistrictDTO district, string token);
        Task DeleteDistrict(Guid gid, string token);
        #endregion

        #region Resource
        Task<IEnumerable<ResourceDTO>> GetResources(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<ResourceDTO> GetResourceByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<ResourceDTO> CreateResource(CreateResourceDTO resource, string token);
        Task UpdateResource(ResourceDTO resource, string token);
        Task DeleteResource(Guid gid, string token);
        #endregion

        #region MeasurementUnit
        Task<IEnumerable<MeasurementUnitDTO>> GetMeasurementUnits(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<MeasurementUnitDTO> GetMeasurementUnitByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<MeasurementUnitDTO> CreateMeasurementUnit(CreateMeasurementUnitDTO measurementUnit, string token);
        Task UpdateMeasurementUnit(MeasurementUnitDTO measurementUnit, string token);
        Task DeleteMeasurementUnit(Guid gid, string token);
        #endregion

        #region ResourceMeasurementUnit
        Task<IEnumerable<ResourceMeasurementUnitDTO>> GetResourcesByUnitGID(Guid unitGid, CancellationToken cancellationToken, string token);
        Task<IEnumerable<ResourceMeasurementUnitDTO>> GetUnitsByResourceGID(Guid resourceGid, CancellationToken cancellationToken, string token);
        Task<ResourceMeasurementUnitDTO> GetResourceUnitByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<ResourceMeasurementUnitDTO> AddResourceToUnit(CreateResourceUnitDTO resourceUnit, string token);
        Task RemoveResourceFromUnit(Guid gid, string token);
        Task<IsExistModel> IsResourceInUnit(ResourceMeasurementUnitDTO resourceMeasurement, string token);
        #endregion
    }
}
