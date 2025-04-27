using AutoMapper;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Volunteers;
using Gateway.DTO.DTOs.Volunteers.Create;
using Gateway.DTO.DTOs.Volunteers.Request;
using Gateway.DTO.DTOs.Volunteers.Update;
using Gateway.DTO.Exceptions;

namespace Gateway.Infrastructure.Services.Services
{
    public class VolunteersService : IVolunteersService
    {
        private IVolunteersGateway _volunteersGateway;
        private IOperationsGateway _operationsGateway;
        private readonly IMapper _mapper;
        public VolunteersService(IVolunteersGateway volunteersGateway, IOperationsGateway operationsGateway, IMapper mapper)
        {
            _volunteersGateway = volunteersGateway;
            _operationsGateway = operationsGateway;
            _mapper = mapper;
        }

        public async Task UpdateVolunteerRating(UpdateVolunteerRatingRequest request, string token)
        {
            var volunteer = await _volunteersGateway.GetVolunteerByGID(request.VolunteerGID, CancellationToken.None, token);
            if (volunteer == null)
                throw new ServiceException("Volunteer not found");

            var newRating = volunteer.RatingNumber + request.RatingNumber;

            if (newRating < 0 || newRating > 100)
                throw new ServiceException("Rating cannot be less than 0 or greater than 100!");

            volunteer.RatingNumber = newRating;

            await _volunteersGateway.UpdateVolunteer(_mapper.Map<UpdateVolunteerDTO>(volunteer), token);
        }

        public async Task<IEnumerable<VolunteerDTO>> GetVolunteersForGroup(Guid groupGID, CancellationToken cancellationToken, string token)
        {
            var volunteersGroups = await _volunteersGateway.GetVolunteersByGroupGID(groupGID, cancellationToken, token);

            var volunteers = await _volunteersGateway.GetVolunteers(new DTO.DTOs.Common.PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);

            return volunteers.Where(v => volunteersGroups.Any(vg => vg.VolunteerGID == v.GID));
        }

        public async Task<IEnumerable<VolunteerDTO>> GetVolunteersForEvent(VolunteersForEventRequest request, CancellationToken cancellationToken, string token)
        {
            var volunteers = await _volunteersGateway.GetVolunteers(new DTO.DTOs.Common.PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);
            var volunteersEvent = (await _volunteersGateway.GetVolunteersByEventGID(request.EventGID, cancellationToken, token)) ?? new List<VolunteersEventsDTO>();

            var volunteersForEvent = volunteers.Where(x => volunteersEvent.Any(y => y.VolunteerGID == x.GID)).ToList();
            if (request.NotInGroup)
            {
                var groups = (await _operationsGateway.GetGroups(new DTO.DTOs.Common.PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token)) ?? new List<GroupDTO>();
                groups = groups.Where(x => x.EventGID == request.EventGID).ToList();


                var volunteersGroups = (await _volunteersGateway.GetVolunteersGroups(new DTO.DTOs.Common.PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token)) ?? new List<VolunteersGroupsDTO>();
                volunteersGroups = volunteersGroups.Where(x => groups.Any(y => y.GID == x.GroupGID)).ToList();

                var volunteersInGroupsGIDs = volunteersGroups.Select(x => x.VolunteerGID).ToList();

                volunteersForEvent = volunteersForEvent.Where(x => !volunteersInGroupsGIDs.Contains(x.GID)).ToList();
            }

            return volunteersForEvent;
        }
        public async Task RemoveVolunteerFromGroup(CreateVolunteersGroupsDTO dto, string token)
        {
            if (!(await _volunteersGateway.IsVolunteerinGroup(dto, token)).IsExist)
                throw new Exception("Volunteer is not in group");

            var volunteersGroups = await _volunteersGateway.GetVolunteersGroups(new DTO.DTOs.Common.PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token);

            var volunteersGroupsGID = volunteersGroups.First(x => x.GroupGID == dto.GroupGID && x.VolunteerGID == dto.VolunteerGID).GID;

            await _volunteersGateway.RemoveVolunteerFromGroup(volunteersGroupsGID, token);
        }
        public async Task<VolunteerDTO?> GeVolunteerByUserGID(Guid userGID, CancellationToken cancellationToken, string token)
        {
            var volunteers = await _volunteersGateway.GetVolunteers(new DTO.DTOs.Common.PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);
            return volunteers.FirstOrDefault(x => x.UserGID == userGID);
        }
    }
}
