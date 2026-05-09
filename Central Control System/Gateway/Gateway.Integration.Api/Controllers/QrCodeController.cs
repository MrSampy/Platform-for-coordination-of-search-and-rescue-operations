using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.Integration.Api.Config;
using Gateway.Integration.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Integration.Api.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("gateway.integration.api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "QrCode")]
    public class QrCodeController : ControllerBase
    {
        private readonly IQRCodeService _qrCodeService;
        public QrCodeController(IQRCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }

        [HttpPost("generate-qr")]
        public async Task<IActionResult> GenerateQCode([FromBody] VolunteerQrRequest model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _qrCodeService.GenerateQrCode(model, token));
        }
    }
}
