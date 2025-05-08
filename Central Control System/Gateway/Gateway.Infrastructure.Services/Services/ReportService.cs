using AutoMapper;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Detail;
using Gateway.DTO.DTOs.Operations.Response;
using Gateway.DTO.DTOs.Volunteers;
using Gateway.DTO.Exceptions;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Gateway.Infrastructure.Services.Services
{
    public class ReportService : IReportService
    {
        public readonly IOperationsGateway _operationsGateway;
        public readonly IVolunteersGateway _volunteerGateway;
        public readonly IMapper _mapper;
        public readonly IEfficiencyCalculationService _efficiencyCalculationService;
        public ReportService(IOperationsGateway operationsGateway, IVolunteersGateway volunteerGateway, IMapper mapper, IEfficiencyCalculationService efficiencyCalculationService)
        {
            _operationsGateway = operationsGateway;
            _volunteerGateway = volunteerGateway;
            _mapper = mapper;
            _efficiencyCalculationService = efficiencyCalculationService;
        }

        public async Task<GetReportResponse> GenerateEventReport(Guid eventGID, CancellationToken cancellationToken, string token)
        {
            var eventDto = await _operationsGateway.GetEventByGID(eventGID, CancellationToken.None, token);

            if (eventDto == null)
                throw new ServiceException("Event not found");

            var groups = (await _operationsGateway.GetGroups(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)).Where(g => g.EventGID == eventGID);

            var volunteersGroups = groups.Count() != 0
                ? (await _volunteerGateway.GetVolunteersGroups(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)).Where(vg => groups.Any(g => g.GID == vg.GroupGID))
            : new List<VolunteersGroupsDTO>();
            var volunteers = volunteersGroups.Count() != 0 ?
                (await _volunteerGateway.GetVolunteers(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)).Where(v => volunteersGroups.Any(vg => vg.VolunteerGID == v.GID))
                : new List<VolunteerDTO>();

            var operationTasks = groups.Count() != 0
                ? (await _operationsGateway.GetOperationTasks(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)).Where(ot => groups.Any(vg => vg.GID == ot.GroupGID))
                : new List<OperationTaskDTO>();
            var resources = (await _operationsGateway.GetResourcesByEventGID(eventGID, CancellationToken.None, token)) ?? new List<ResourcesEventDTO>();


            var eventDetail = await ConvertToDetailEvent(eventDto, cancellationToken, token);

            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "Fonts", "Times New Roman.ttf");
            var boldFontPath = Path.Combine(Directory.GetCurrentDirectory(), "Fonts", "Times New Roman Bold.ttf");

            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
            var boldFont = PdfFontFactory.CreateFont(boldFontPath, PdfEncodings.IDENTITY_H);

            // Title
            document.Add(new Paragraph("Звіт про операцію")
                .SetFont(boldFont)
                .SetFontSize(20)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            // SECTION: Event Info
            document.Add(new Paragraph("Інформація про операцію")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetMarginBottom(5));

            document.Add(CreateLabelValueParagraph("Назва операції:", eventDto.Name, font, boldFont));
            document.Add(CreateLabelValueParagraph("Тип операції:", eventDetail.EventType, font, boldFont));
            document.Add(CreateLabelValueParagraph("Статус операції:", eventDetail.EventStatus, font, boldFont));
            document.Add(CreateLabelValueParagraph("Район:", eventDetail.District, font, boldFont));
            document.Add(CreateLabelValueParagraph("Координатор:", eventDetail.Coordinator, font, boldFont));
            document.Add(CreateLabelValueParagraph("Диспетчер:", eventDetail.Dispatcher, font, boldFont));
            document.Add(CreateLabelValueParagraph("Дата створення:", eventDto.CreatedAt.ToString("dd.MM.yyyy HH:mm"), font, boldFont));
            document.Add(CreateLabelValueParagraph("Дата оновлення:", eventDto.UpdatedAt.ToString("dd.MM.yyyy HH:mm"), font, boldFont));

            AddSeparator(document);

            // SECTION: Coordinates
            document.Add(new Paragraph("Географічні координати")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetMarginBottom(5));

            document.Add(CreateLabelValueParagraph("Широта:", eventDto.Latitude.ToString("F6"), font, boldFont));
            document.Add(CreateLabelValueParagraph("Довгота:", eventDto.Longitude.ToString("F6"), font, boldFont));

            AddSeparator(document);

            // SECTION: Volunteer Groups
            if (groups.Any())
            {
                document.Add(new Paragraph("Групи волонтерів")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetMarginBottom(5));

                foreach (var group in groups)
                {
                    var leader = volunteers.FirstOrDefault(l => l.GID == group.LeaderGID);
                    var leaderName = leader != null ? $"{leader.Surname} {leader.Name} {leader.SecondName}" : "-";

                    document.Add(CreateLabelValueParagraph("Назва групи:", group.Name, font, boldFont));
                    document.Add(CreateLabelValueParagraph("Керівник:", leaderName, font, boldFont));
                    AddSeparator(document);
                }
            }

            // SECTION: Завдання
            if (operationTasks.Any())
            {
                document.Add(new Paragraph("Завдання для груп")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetMarginBottom(5));

                foreach (var task in operationTasks)
                {
                    document.Add(CreateLabelValueParagraph("Назва завдання:", task.Name, font, boldFont));
                    document.Add(CreateLabelValueParagraph("Тип завдання:", SharedConstants.OperationTaskStatuses.First(o => o.GID == task.TaskStatusGID).Name, font, boldFont));
                    document.Add(CreateLabelValueParagraph("Опис:", task.TaskDescription, font, boldFont));
                    AddSeparator(document);
                }
            }

            // SECTION: Волонтери
            if (volunteers.Any())
            {
                document.Add(new Paragraph("Учасники груп")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetMarginBottom(5));

                foreach (var v in volunteers)
                {
                    var fullName = $"{v.Surname} {v.Name} {v.SecondName}";
                    document.Add(CreateLabelValueParagraph("ПІБ:", fullName, font, boldFont));
                    document.Add(CreateLabelValueParagraph("Телефон:", v.MobilePhone, font, boldFont));
                    document.Add(CreateLabelValueParagraph("Email:", v.Email, font, boldFont));
                    document.Add(CreateLabelValueParagraph("Дата народження:", v.BirthDate.ToString("dd.MM.yyyy"), font, boldFont));
                    AddSeparator(document);
                }
            }

            if (resources.Any())
            {
                document.Add(new Paragraph("Ресурси операції")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetMarginBottom(5));

                foreach (var resource in resources)
                {
                    var measureName = SharedConstants.MeasurementUnits.FirstOrDefault(x => x.GID == resource.MeasurementUnitGID)?.Name;
                    var resourceName = SharedConstants.Resources.FirstOrDefault(x => x.GID == resource.ResourceGID)?.Name;

                    document.Add(CreateLabelValueParagraph($"{resourceName}:", $"{Math.Round(resource.AvailableQuantity, 2)}/{Math.Round(resource.RequiredQuantity, 2)} ({measureName})", font, boldFont));
                }
            }

            var efficiency = await _efficiencyCalculationService.CalculateEfficiency(eventDetail.EventType, volunteers.Count(),
                operationTasks.Where(t => t.TaskStatusGID == SharedConstants.TaskStatusCompleted).Count(),
                operationTasks.Count(), resources.Select(r => r.AvailableQuantity).Sum(),
                resources.Select(r => r.RequiredQuantity).Sum(), cancellationToken, token);

            document.Add(new Paragraph($"Efficiency: {efficiency}%.")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetMarginBottom(5));

            document.Close();

            return new GetReportResponse
            {
                Bytes = ms.ToArray(),
                FileName = $"Report_{eventDto.Name}.pdf"
            };
        }

        private Paragraph CreateLabelValueParagraph(string label, string value, PdfFont font, PdfFont boldFont)
        {
            return new Paragraph()
                .Add(new Text(label + " ").SetFont(boldFont))
                .Add(new Text(value).SetFont(font))
                .SetMarginBottom(4);
        }

        private void AddSeparator(Document doc)
        {
            var line = new LineSeparator(new SolidLine())
                .SetMarginTop(10)
                .SetMarginBottom(10);
            doc.Add(line);
        }

        private async Task<EventDetails> ConvertToDetailEvent(EventDTO eventDTO, CancellationToken cancellationToken, string token)
        {
            var dispatcher = await _operationsGateway.GetOperationWorkerByGID(eventDTO.DispatcherGID, cancellationToken, token);
            var coordinator = await _operationsGateway.GetOperationWorkerByGID(eventDTO.CoordinatorGID, cancellationToken, token);

            var clearEvent = _mapper.Map<EventDetails>(eventDTO);

            clearEvent.Dispatcher = $"{dispatcher.Name} {dispatcher.Surname} {dispatcher.SecondName}";
            clearEvent.Coordinator = $"{coordinator.Name} {coordinator.Surname} {coordinator.SecondName}";
            clearEvent.EventStatus = SharedConstants.EventStatuses.FirstOrDefault(x => x.GID == eventDTO.EventStatusGID)?.Name!;
            clearEvent.EventType = SharedConstants.EventTypes.FirstOrDefault(x => x.GID == eventDTO.EventTypeGID)?.Name!;
            clearEvent.District = SharedConstants.Districts.FirstOrDefault(x => x.GID == eventDTO.DistrictGID)?.Name!;

            return clearEvent;
        }
    }
}
