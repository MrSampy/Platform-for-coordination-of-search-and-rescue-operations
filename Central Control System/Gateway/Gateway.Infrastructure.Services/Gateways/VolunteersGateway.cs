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

        public VolunteersGateway(IApiBuilder apiBuilder)
        {
            _apiBuilder = apiBuilder;
        }

        #region Volunteer

        public async Task<IEnumerable<VolunteerDTO>> GetVolunteers(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            string query = $"volunteers/api/Volunteer?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            return await _apiBuilder.GetRequest<IEnumerable<VolunteerDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteerDTO> GetVolunteerByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<VolunteerDTO>($"volunteers/api/Volunteer/{gid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteerDTO> CreateVolunteer(CreateVolunteerDTO volunteer, string token)
        {
            return await _apiBuilder.PostRequest<VolunteerDTO>("volunteers/api/Volunteer", volunteer, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task UpdateVolunteer(UpdateVolunteerDTO volunteer, string token)
        {
            await _apiBuilder.PutRequestWithoutDeserializing("volunteers/api/Volunteer", volunteer, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task DeleteVolunteer(Guid gid, string token)
        {
            await _apiBuilder.DeleteRequest($"volunteers/api/Volunteer/{gid}", SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        #endregion

        #region VolunteersDistricts

        public async Task<IEnumerable<VolunteersDistrictsDTO>> GetVolunteersDistricts(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            string query = $"volunteers/api/VolunteersDistricts?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            return await _apiBuilder.GetRequest<IEnumerable<VolunteersDistrictsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<IEnumerable<VolunteersDistrictsDTO>> GetDistrictsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<IEnumerable<VolunteersDistrictsDTO>>(
                $"volunteers/api/VolunteersDistricts/by-volunteer/{volunteerGid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<IEnumerable<VolunteersDistrictsDTO>> GetVolunteersByDistrictGID(Guid districtGID, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<IEnumerable<VolunteersDistrictsDTO>>(
                $"volunteers/api/VolunteersDistricts/by-district/{districtGID}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteersDistrictsDTO> GetVolunteersDistrictByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<VolunteersDistrictsDTO>(
                $"volunteers/api/VolunteersDistricts/{gid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteersDistrictsDTO> CreateVolunteersDistrict(CreateVolunteersDistrictsDTO volunteersDistricts, string token)
        {
            return await _apiBuilder.PostRequest<VolunteersDistrictsDTO>(
                "volunteers/api/VolunteersDistricts", volunteersDistricts, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task DeleteVolunteersDistrict(Guid gid, string token)
        {
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
            string query = $"volunteers/api/VolunteersGroups?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
            return await _apiBuilder.GetRequest<IEnumerable<VolunteersGroupsDTO>>(query, SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<IEnumerable<VolunteersGroupsDTO>> GetGroupsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<IEnumerable<VolunteersGroupsDTO>>(
                $"volunteers/api/VolunteersGroups/by-volunteer/{volunteerGid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<IEnumerable<VolunteersGroupsDTO>> GetVolunteersByGroupGID(Guid groupGid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<IEnumerable<VolunteersGroupsDTO>>(
                $"volunteers/api/VolunteersGroups/by-group/{groupGid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteersGroupsDTO> GetVolunteersGroupsByGID(Guid gid, CancellationToken cancellationToken, string token)
        {
            return await _apiBuilder.GetRequest<VolunteersGroupsDTO>(
                $"volunteers/api/VolunteersGroups/{gid}", SharedConstants.VolunteerService, cancellationToken, token);
        }

        public async Task<VolunteersGroupsDTO> AddVolunteerToGroup(CreateVolunteersDistrictsDTO volunteersGroups, string token)
        {
            return await _apiBuilder.PostRequest<VolunteersGroupsDTO>(
                "volunteers/api/VolunteersGroups", volunteersGroups, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task RemoveVolunteerFromGroup(Guid gid, string token)
        {
            await _apiBuilder.DeleteRequest($"volunteers/api/VolunteersGroups/{gid}", SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        public async Task<IsExistModel> IsVolunteerinGroup(CreateVolunteersGroupsDTO volunteersGroups, string token)
        {
            return await _apiBuilder.PostRequest<IsExistModel>(
                "volunteers/api/VolunteersGroups/exists", volunteersGroups, SharedConstants.VolunteerService, CancellationToken.None, token);
        }

        #endregion
    }

}
