using Gateway.DTO.DTOs.Operations.Response;

namespace Gateway.Domain.Services.Interfaces
{
    public interface IReportService
    {
        Task<GetReportResponse> GenerateEventReport(Guid eventGID, CancellationToken cancellationToken, string token);
    }
}
