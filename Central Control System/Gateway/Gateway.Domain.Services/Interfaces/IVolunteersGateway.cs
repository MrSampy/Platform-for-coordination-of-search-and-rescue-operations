using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Volunteers;
using Gateway.DTO.DTOs.Volunteers.Create;
using Gateway.DTO.DTOs.Volunteers.Update;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IVolunteersGateway
    {
        #region Volunteer
        Task<IEnumerable<VolunteerDTO>> GetVolunteers(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<VolunteerDTO> GetVolunteerByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<VolunteerDTO> CreateVolunteer(CreateVolunteerDTO volunteer, string token);
        Task UpdateVolunteer(UpdateVolunteerDTO volunteer, string token);
        Task DeleteVolunteer(Guid gid, string token);
        #endregion

        #region VolunteersDistricts
        Task<IEnumerable<VolunteersDistrictsDTO>> GetVolunteersDistricts(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<IEnumerable<VolunteersDistrictsDTO>> GetDistrictsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken, string token);
        Task<IEnumerable<VolunteersDistrictsDTO>> GetVolunteersByDistrictGID(Guid districtGID, CancellationToken cancellationToken, string token);
        Task<VolunteersDistrictsDTO> GetVolunteersDistrictByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<VolunteersDistrictsDTO> CreateVolunteersDistrict(CreateVolunteersDistrictsDTO volunteersDistricts, string token);
        Task DeleteVolunteersDistrict(Guid gid, string token);
        Task<IsExistModel> IsVolunteerinDistrict(CreateVolunteersDistrictsDTO volunteersDistricts, string token);
        #endregion

        #region VolunteersGroups
        Task<IEnumerable<VolunteersGroupsDTO>> GetVolunteersGroups(PaginationQuery paginationQuery, CancellationToken cancellationToken, string token);
        Task<IEnumerable<VolunteersGroupsDTO>> GetGroupsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken, string token);
        Task<IEnumerable<VolunteersGroupsDTO>> GetVolunteersByGroupGID(Guid groupGid, CancellationToken cancellationToken, string token);
        Task<VolunteersGroupsDTO> GetVolunteersGroupsByGID(Guid gid, CancellationToken cancellationToken, string token);
        Task<VolunteersGroupsDTO> AddVolunteerToGroup(CreateVolunteersGroupsDTO volunteersGroups, string token);
        Task RemoveVolunteerFromGroup(Guid gid, string token);
        Task<IsExistModel> IsVolunteerinGroup(CreateVolunteersGroupsDTO volunteersGroups, string token);
        #endregion
    }
}
