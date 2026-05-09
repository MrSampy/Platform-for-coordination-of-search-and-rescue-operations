using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Response;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IQRCodeService
    {
        Task<VolunteerQrResponse> GenerateQrCode(VolunteerQrRequest request, string token);
    }
}
