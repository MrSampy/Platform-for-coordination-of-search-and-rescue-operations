using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilsService.API.Model;
using UtilsService.Application.Commands.MeasurementUnitCommands.DeleteMeasurementUnit;
using UtilsService.Application.Commands.MeasurementUnitCommands.UpdateMeasurementUnit;
using UtilsService.Application.Queries.MeasurementUnitQueries.CreateMeasurementUnit;
using UtilsService.Application.Queries.MeasurementUnitQueries.GetAllMeasurementUnits;
using UtilsService.Application.Queries.MeasurementUnitQueries.GetMeasurementUnitByGid;
using UtilsService.Domain.Entities;

namespace UtilsService.API.Controllers
{
    [ApiController]
    [Route("utils/api/[controller]")]
    public class MeasurementUnitController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MeasurementUnitController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
        public async Task<IActionResult> GetMeasurementUnits([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllMeasurementUnitsQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
        public async Task<IActionResult> GetMeasurementUnitByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetMeasurementUnitByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeasurementUnit([FromBody] MeasurementUnit measurementUnit)
        {
            return Ok(await _mediator.Send(new CreateMeasurementUnitQuery() { MeasurementUnit = measurementUnit }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMeasurementUnit([FromBody] MeasurementUnit measurementUnit)
        {
            await _mediator.Send(new UpdateMeasurementUnitCommand() { MeasurementUnit = measurementUnit });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteMeasurementUnit(Guid gid)
        {
            await _mediator.Send(new DeleteMeasurementUnitCommand() { GID = gid });
            return NoContent();
        }
    }
}