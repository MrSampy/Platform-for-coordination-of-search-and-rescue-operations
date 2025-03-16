using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerService.API.Config;
using VolunteerService.API.Model;
using VolunteerService.Application.Commands.VolunteersDistrictsCommands.Delete;
using VolunteerService.Application.Commands.VolunteersDistrictsCommands.Update;
using VolunteerService.Application.DTOs.Create;
using VolunteerService.Application.DTOs.Update;
using VolunteerService.Application.Queries.VolunteersDistrictsQueries.Create;
using VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetAll;
using VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetByGID;
using VolunteerService.Domain.Entities;

namespace VolunteerService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("volunteers/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "VolunteersDistricts")]
    public class VolunteersDistrictsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VolunteersDistrictsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetVolunteersDistricts([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllVolunteersDistrictsQuery { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteersDistrictByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetVolunteersDistrictByGidQuery { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateVolunteersDistrict([FromBody] CreateVolunteersDistrictsDTO districtDto)
        {
            return Ok(await _mediator.Send(new CreateVolunteersDistrictQuery { VolunteersDistrictDTO = districtDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVolunteersDistrict([FromBody] UpdateVolunteersDistrictsDTO districtModel)
        {
            await _mediator.Send(new UpdateVolunteersDistrictCommand { VolunteersDistrictDTO = districtModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteVolunteersDistrict(Guid gid)
        {
            await _mediator.Send(new DeleteVolunteersDistrictCommand { GID = gid });
            return NoContent();
        }
    }
}
