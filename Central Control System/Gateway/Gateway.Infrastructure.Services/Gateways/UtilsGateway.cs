using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Utils;
using Gateway.DTO.DTOs.Utils.Create;

namespace Gateway.Infrastructure.Services.Gateways
{
    namespace Gateway.Infrastructure.Services.Gateways
    {
        public class UtilsGateway : IUtilsGateway
        {
            private readonly IApiBuilder _apiBuilder;
            private readonly ICacheService<DistrictDTO> _districtCache;
            private readonly ICacheService<ResourceDTO> _resourceCache;
            private readonly ICacheService<MeasurementUnitDTO> _measurementUnitCache;

            public UtilsGateway(
                IApiBuilder apiBuilder,
                ICacheService<DistrictDTO> districtCache,
                ICacheService<ResourceDTO> resourceCache,
                ICacheService<MeasurementUnitDTO> measurementUnitCache
            )
            {
                _apiBuilder = apiBuilder;
                _districtCache = districtCache;
                _resourceCache = resourceCache;
                _measurementUnitCache = measurementUnitCache;
            }

            #region District

            public async Task<IEnumerable<DistrictDTO>> GetDistricts(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
            {
                var cached = _districtCache.Get(paginationQuery.GetKey());
                if (cached != null)
                    return cached;

                var query = $"utils/api/District?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
                var result = await _apiBuilder.GetRequest<IEnumerable<DistrictDTO>>(query, SharedConstants.UtilsService, cancellationToken, token);

                _districtCache.Set(paginationQuery.GetKey(), result.ToList());
                return result;
            }

            public async Task<DistrictDTO> GetDistrictByGID(Guid gid, CancellationToken cancellationToken, string token)
            {
                return await _apiBuilder.GetRequest<DistrictDTO>($"utils/api/District/{gid}", SharedConstants.UtilsService, cancellationToken, token);
            }

            public async Task<DistrictDTO> CreateDistrict(CreateDistrictDTO district, string token)
            {
                var result = await _apiBuilder.PostRequest<DistrictDTO>("utils/api/District", district, SharedConstants.UtilsService, CancellationToken.None, token);
                _districtCache.Reset();
                return result;
            }

            public async Task UpdateDistrict(DistrictDTO district, string token)
            {
                await _apiBuilder.PutRequestWithoutDeserializing("utils/api/District", district, SharedConstants.UtilsService, CancellationToken.None, token);
                _districtCache.Reset();
            }

            public async Task DeleteDistrict(Guid gid, string token)
            {
                await _apiBuilder.DeleteRequest($"utils/api/District/{gid}", SharedConstants.UtilsService, CancellationToken.None, token);
                _districtCache.Reset();
            }

            #endregion

            #region Resource

            public async Task<IEnumerable<ResourceDTO>> GetResources(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
            {
                var cached = _resourceCache.Get(paginationQuery.GetKey());
                if (cached != null)
                    return cached;

                var query = $"utils/api/Resource?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
                var result = await _apiBuilder.GetRequest<IEnumerable<ResourceDTO>>(query, SharedConstants.UtilsService, cancellationToken, token);

                _resourceCache.Set(paginationQuery.GetKey(), result.ToList());
                return result;
            }

            public async Task<ResourceDTO> GetResourceByGID(Guid gid, CancellationToken cancellationToken, string token)
            {
                return await _apiBuilder.GetRequest<ResourceDTO>($"utils/api/Resource/{gid}", SharedConstants.UtilsService, cancellationToken, token);
            }

            public async Task<ResourceDTO> CreateResource(CreateResourceDTO resource, string token)
            {
                var result = await _apiBuilder.PostRequest<ResourceDTO>("utils/api/Resource", resource, SharedConstants.UtilsService, CancellationToken.None, token);
                _resourceCache.Reset();
                return result;
            }

            public async Task UpdateResource(ResourceDTO resource, string token)
            {
                await _apiBuilder.PutRequestWithoutDeserializing("utils/api/Resource", resource, SharedConstants.UtilsService, CancellationToken.None, token);
                _resourceCache.Reset();
            }

            public async Task DeleteResource(Guid gid, string token)
            {
                await _apiBuilder.DeleteRequest($"utils/api/Resource/{gid}", SharedConstants.UtilsService, CancellationToken.None, token);
                _resourceCache.Reset();
            }

            #endregion

            #region MeasurementUnit

            public async Task<IEnumerable<MeasurementUnitDTO>> GetMeasurementUnits(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
            {
                var cached = _measurementUnitCache.Get(paginationQuery.GetKey());
                if (cached != null)
                    return cached;

                var query = $"utils/api/MeasurementUnit?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
                var result = await _apiBuilder.GetRequest<IEnumerable<MeasurementUnitDTO>>(query, SharedConstants.UtilsService, cancellationToken, token);

                _measurementUnitCache.Set(paginationQuery.GetKey(), result.ToList());
                return result;
            }

            public async Task<MeasurementUnitDTO> GetMeasurementUnitByGID(Guid gid, CancellationToken cancellationToken, string token)
            {
                return await _apiBuilder.GetRequest<MeasurementUnitDTO>($"utils/api/MeasurementUnit/{gid}", SharedConstants.UtilsService, cancellationToken, token);
            }

            public async Task<MeasurementUnitDTO> CreateMeasurementUnit(CreateMeasurementUnitDTO measurementUnit, string token)
            {
                var result = await _apiBuilder.PostRequest<MeasurementUnitDTO>("utils/api/MeasurementUnit", measurementUnit, SharedConstants.UtilsService, CancellationToken.None, token);
                _measurementUnitCache.Reset();
                return result;
            }

            public async Task UpdateMeasurementUnit(MeasurementUnitDTO measurementUnit, string token)
            {
                await _apiBuilder.PutRequestWithoutDeserializing("utils/api/MeasurementUnit", measurementUnit, SharedConstants.UtilsService, CancellationToken.None, token);
                _measurementUnitCache.Reset();
            }

            public async Task DeleteMeasurementUnit(Guid gid, string token)
            {
                await _apiBuilder.DeleteRequest($"utils/api/MeasurementUnit/{gid}", SharedConstants.UtilsService, CancellationToken.None, token);
                _measurementUnitCache.Reset();
            }

            #endregion

            #region ResourceMeasurementUnit

            public async Task<IEnumerable<ResourceMeasurementUnitDTO>> GetResourcesByUnitGID(Guid unitGid, CancellationToken cancellationToken, string token)
            {
                return await _apiBuilder.GetRequest<IEnumerable<ResourceMeasurementUnitDTO>>(
                    $"utils/api/ResourceMeasurementUnit/by-unit/{unitGid}", SharedConstants.UtilsService, cancellationToken, token);
            }

            public async Task<IEnumerable<ResourceMeasurementUnitDTO>> GetUnitsByResourceGID(Guid resourceGid, CancellationToken cancellationToken, string token)
            {
                return await _apiBuilder.GetRequest<IEnumerable<ResourceMeasurementUnitDTO>>(
                    $"utils/api/ResourceMeasurementUnit/by-resource/{resourceGid}", SharedConstants.UtilsService, cancellationToken, token);
            }

            public async Task<ResourceMeasurementUnitDTO> GetResourceUnitByGID(Guid gid, CancellationToken cancellationToken, string token)
            {
                return await _apiBuilder.GetRequest<ResourceMeasurementUnitDTO>(
                    $"utils/api/ResourceMeasurementUnit/{gid}", SharedConstants.UtilsService, cancellationToken, token);
            }

            public async Task<ResourceMeasurementUnitDTO> AddResourceToUnit(CreateResourceUnitDTO resourceUnit, string token)
            {
                return await _apiBuilder.PostRequest<ResourceMeasurementUnitDTO>(
                    $"utils/api/ResourceMeasurementUnit", resourceUnit, SharedConstants.UtilsService, CancellationToken.None, token);
            }

            public async Task RemoveResourceFromUnit(Guid gid, string token)
            {
                await _apiBuilder.DeleteRequest($"utils/api/ResourceMeasurementUnit/{gid}", SharedConstants.UtilsService, CancellationToken.None, token);
            }

            public async Task<IsExistModel> IsResourceInUnit(ResourceMeasurementUnitDTO resourceMeasurement, string token)
            {
                return await _apiBuilder.PostRequest<IsExistModel>(
                    "utils/api/ResourceMeasurementUnit/exists", resourceMeasurement, SharedConstants.UtilsService, CancellationToken.None, token);
            }

            #endregion
        }
    }

}
