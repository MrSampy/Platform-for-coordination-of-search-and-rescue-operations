using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Response;
using Newtonsoft.Json;
using QRCoder;

namespace Gateway.Infrastructure.Services.Services
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IVolunteersGateway _volunteersGateway;

        public QRCodeService(IOperationsGateway operationsGateway, IVolunteersGateway volunteersGateway)
        {
            _operationsGateway = operationsGateway;
            _volunteersGateway = volunteersGateway;
        }

        public async Task<VolunteerQrResponse> GenerateQrCode(VolunteerQrRequest request, string token)
        {
            var volunteer = await _volunteersGateway.GetVolunteerByGID(request.VolunteerGID, CancellationToken.None, token);
            if (volunteer == null)
            {
                throw new ArgumentException("Volunteer not found");
            }

            var eventDetails = await _operationsGateway.GetEventByGID(request.EventGID, CancellationToken.None, token);
            if (eventDetails == null)
            {
                throw new ArgumentException("Event not found");
            }

            var payload = new
            {
                volunteerGID = request.VolunteerGID,
                eventGID = request.EventGID
            };

            var json = JsonConvert.SerializeObject(payload);

            // Генерація QR
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeAsPng = qrCode.GetGraphic(20);

            var base64Qr = Convert.ToBase64String(qrCodeAsPng);

            return new VolunteerQrResponse { QRBase64 = base64Qr };
        }
    }
}
