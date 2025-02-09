using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilsService.API.Model;
using UtilsService.Application.Commands.DistrictCommands.DeleteDistrict;
using UtilsService.Application.Commands.DistrictCommands.UpdateDistrict;
using UtilsService.Application.Queries.DistrictQueries.CreateDistrict;
using UtilsService.Application.Queries.DistrictQueries.GetAllDistricts;
using UtilsService.Application.Queries.DistrictQueries.GetDistrictByGid;
using UtilsService.Domain.Entities;

namespace UtilsService.API.Controllers
{
    [ApiController]
    [Route("utils/api/[controller]")]
    public class DistrictController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DistrictController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
        public async Task<IActionResult> GetDistricts([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllDistrictsQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
        public async Task<IActionResult> GetDistrictByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetDistrictByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDistrict([FromBody] District district)
        {
            return Ok(await _mediator.Send(new CreateDistrictQuery() { District = district }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDistrict([FromBody] District district)
        {
            await _mediator.Send(new UpdateDistrictCommand() { District = district });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteDistrict(Guid gid)
        {
            await _mediator.Send(new DeleteDistrictCommand() { GID = gid });
            return NoContent();
        }
    }
}
