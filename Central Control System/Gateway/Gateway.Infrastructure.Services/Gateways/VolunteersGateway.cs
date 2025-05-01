using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Volunteers;
using Gateway.DTO.DTOs.Volunteers.Create;
using Gateway.DTO.DTOs.Volunteers.Update;

namespace Gateway.Infrastructure.Services.Gateways
{
    public class VolunteersGateway : IVolunteersGateway
    {
        private readonly IApiBuilder _apiBuilder;
        private readonly ICacheService<VolunteerDTO> _volunteerCache;
        private readonly ICacheService<VolunteersDistrictsDTO> _volunteersDistrictsCache;
        private readonly ICacheService<VolunteersGroupsDTO> _volunteersGroupsCache;
        private readonly ICacheService<VolunteersEventsDTO> _volunteersEventsCache;

        public VolunteersGateway(IApiBuilder apiBuilder, ICacheService<VolunteerDTO> volunteerCache,
            ICacheService<VolunteersDistrictsDTO> volunteersDistrictsCache,
            ICacheService<VolunteersGroupsDTO> volunteersGroupsCache,
            ICacheService<VolunteersEventsDTO> volunteersEventsCache)
        {
            _apiBuilder = apiBuilder;
            _volunteerCache = volunteerCache;
            _volunteersDistrictsCache = volunteersDistrictsCache;
            _volunteersGroupsCache = volunteersGroupsCache;
            _volunteersEventsCache = volunteersEventsCache;
        }

        #region Volunteer

        public async Task<IEnumerable<VolunteerDTO>> GetVolunteers(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteerCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var query = $"volunteers/api/Volunteer?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteerDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteerCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<VolunteerDTO> GetVolunteerByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<VolunteerDTO>($"volunteers/api/Volunteer/{gid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteerDTO> CreateVolunteer(CreateVolunteerDTO volunteer, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteerDTO));
            return await _apiBuilder.PostRequest<VolunteerDTO>("volunteers/api/Volunteer", volunteer, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task UpdateVolunteer(UpdateVolunteerDTO volunteer, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteerDTO));
            await _apiBuilder.PutRequestWithoutDeserializing("volunteers/api/Volunteer", volunteer, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task DeleteVolunteer(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteerDTO));
            await _apiBuilder.DeleteRequest($"volunteers/api/Volunteer/{gid}", SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        #endregion

        #region VolunteersDistricts

        public async Task<IEnumerable<VolunteersDistrictsDTO>> GetVolunteersDistricts(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersDistrictsCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersDistricts?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersDistrictsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersDistrictsCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<IEnumerable<VolunteersDistrictsDTO>> GetDistrictsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersDistrictsCache.Get($"{nameof(GetDistrictsByVolunteerGID)}:{volunteerGid}");
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersDistricts/by-volunteer/{volunteerGid}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersDistrictsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersDistrictsCache.Set($"{nameof(GetDistrictsByVolunteerGID)}:{volunteerGid}", result.ToList());
            return result;
        }

        public async Task<IEnumerable<VolunteersDistrictsDTO>> GetVolunteersByDistrictGID(Guid districtGID, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersDistrictsCache.Get($"{nameof(GetVolunteersByDistrictGID)}:{districtGID}");
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersDistricts/by-district/{districtGID}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersDistrictsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersDistrictsCache.Set($"{nameof(GetVolunteersByDistrictGID)}:{districtGID}", result.ToList());
            return result;
        }

        public async Task<VolunteersDistrictsDTO> GetVolunteersDistrictByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteersDistrictsDTO));
            return await _apiBuilder.GetRequest<VolunteersDistrictsDTO>(
                $"volunteers/api/VolunteersDistricts/{gid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteersDistrictsDTO> CreateVolunteersDistrict(CreateVolunteersDistrictsDTO volunteersDistricts, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteersDistrictsDTO));
            return await _apiBuilder.PostRequest<VolunteersDistrictsDTO>(
                "volunteers/api/VolunteersDistricts", volunteersDistricts, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task DeleteVolunteersDistrict(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteersDistrictsDTO));
            await _apiBuilder.DeleteRequest($"volunteers/api/VolunteersDistricts/{gid}", SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task<IsExistModel> IsVolunteerinDistrict(CreateVolunteersDistrictsDTO volunteersDistricts, string token)
        {
            return await _apiBuilder.PostRequest<IsExistModel>(
                "volunteers/api/VolunteersDistricts/exists", volunteersDistricts, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        #endregion

        #region VolunteersGroups

        public async Task<IEnumerable<VolunteersGroupsDTO>> GetVolunteersGroups(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersGroupsCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersGroups?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersGroupsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersGroupsCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<IEnumerable<VolunteersGroupsDTO>> GetGroupsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersGroupsCache.Get($"{nameof(GetGroupsByVolunteerGID)}:{volunteerGid}");
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersGroups/by-volunteer/{volunteerGid}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersGroupsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersGroupsCache.Set($"{nameof(GetGroupsByVolunteerGID)}:{volunteerGid}", result.ToList());
            return result;
        }

        public async Task<IEnumerable<VolunteersGroupsDTO>> GetVolunteersByGroupGID(Guid groupGid, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersGroupsCache.Get($"{nameof(GetVolunteersByGroupGID)}:{groupGid}");
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersGroups/by-group/{groupGid}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersGroupsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersGroupsCache.Set($"{nameof(GetVolunteersByGroupGID)}:{groupGid}", result.ToList());
            return result;
        }

        public async Task<VolunteersGroupsDTO> GetVolunteersGroupsByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<VolunteersGroupsDTO>($"volunteers/api/VolunteersGroups/{gid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteersGroupsDTO> AddVolunteerToGroup(CreateVolunteersGroupsDTO volunteersGroups, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteersGroupsDTO));
            return await _apiBuilder.PostRequest<VolunteersGroupsDTO>("volunteers/api/VolunteersGroups", volunteersGroups, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task RemoveVolunteerFromGroup(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteersGroupsDTO));
            await _apiBuilder.DeleteRequest($"volunteers/api/VolunteersGroups/{gid}", SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task<IsExistModel> IsVolunteerinGroup(CreateVolunteersGroupsDTO volunteersGroups, string token)
        {
            return await _apiBuilder.PostRequest<IsExistModel>("volunteers/api/VolunteersGroups/exists", volunteersGroups, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        #endregion


        #region VolunteersEvents

        public async Task<IEnumerable<VolunteersEventsDTO>> GetVolunteersEvents(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersEventsCache.Get(paginationQuery.GetKey());
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersEvents?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersEventsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersEventsCache.Set(paginationQuery.GetKey(), result.ToList());
            return result;
        }

        public async Task<IEnumerable<VolunteersEventsDTO>> GetEventsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersEventsCache.Get($"{nameof(GetEventsByVolunteerGID)}:{volunteerGid}");
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersEvents/by-volunteer/{volunteerGid}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersEventsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersEventsCache.Set($"{nameof(GetEventsByVolunteerGID)}:{volunteerGid}", result.ToList());
            return result;
        }

        public async Task<IEnumerable<VolunteersEventsDTO>> GetVolunteersByEventGID(Guid eventGid, CancellationToken cancellationToken, string token)
        {
            var cached = _volunteersEventsCache.Get($"{nameof(GetVolunteersByEventGID)}:{eventGid}");
            if (cached != null)
                return cached;

            var query = $"volunteers/api/VolunteersEvents/by-event/{eventGid}";
            var result = await _apiBuilder.GetRequest<IEnumerable<VolunteersEventsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);

            _volunteersEventsCache.Set($"{nameof(GetVolunteersByEventGID)}:{eventGid}", result.ToList());
            return result;
        }

        public async Task<VolunteersEventsDTO> GetVolunteersEventByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<VolunteersEventsDTO>($"volunteers/api/VolunteersEvents/{gid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteersEventsDTO> AddVolunteerToEvent(CreateVolunteersEventsDTO volunteersEvents, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteersEventsDTO));
            return await _apiBuilder.PostRequest<VolunteersEventsDTO>("volunteers/api/VolunteersEvents", volunteersEvents, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task RemoveVolunteerFromEvent(Guid gid, string token)
        {
            _apiBuilder.SendResetCacheEvent(nameof(VolunteersEventsDTO));
            await _apiBuilder.DeleteRequest($"volunteers/api/VolunteersEvents/{gid}", SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task<IsExistModel> IsVolunteerInEvent(CreateVolunteersEventsDTO volunteersEvents, string token)
        {
            return await _apiBuilder.PostRequest<IsExistModel>("volunteers/api/VolunteersEvents/exists", volunteersEvents, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        #endregion
    }

}
