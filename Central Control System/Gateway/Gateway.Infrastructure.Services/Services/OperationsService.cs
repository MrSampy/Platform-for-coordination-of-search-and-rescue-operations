using AutoMapper;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Detail;
using Gateway.DTO.DTOs.Operations.Request;
using Gateway.DTO.DTOs.Operations.Response;
using Gateway.DTO.DTOs.Operations.Update;
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
    public class OperationsService : IOperationsService
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IMapper _mapper;
        private readonly IAuthGateway _authGateway;
        private readonly IVolunteersGateway _volunteerGroupsGateway;

        public OperationsService(IOperationsGateway operationsGateway, IAuthGateway authGateway, IVolunteersGateway volunteersGateway, IMapper mapper)
        {
            _operationsGateway = operationsGateway;
            _mapper = mapper;
            _authGateway = authGateway;
            _volunteerGroupsGateway = volunteersGateway;
        }
        public async Task<GetAllEntitesReponse<MessageDetail>> GetMessages(MessagePaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var messges = await _operationsGateway.GetMessages(paginationQuery, cancellationToken, token);

            var resultMessages = new List<MessageDetail>();

            foreach (var message in messges.Items)
            {
                var messageDetail = _mapper.Map<MessageDetail>(message);

                var sender = await _operationsGateway.GetOperationWorkerByGID(message.From, cancellationToken, token);
                var receiver = await _operationsGateway.GetOperationWorkerByGID(message.To, cancellationToken, token);

                messageDetail.Sender = $"{sender.Name} {sender.Surname} {sender.SecondName}";
                messageDetail.Receiver = $"{receiver.Name} {receiver.Surname} {receiver.SecondName}";

                resultMessages.Add(messageDetail);
            }

            return new GetAllEntitesReponse<MessageDetail>
            {
                Items = resultMessages,
                TotalCount = messges.TotalCount
            };
        }

        public async Task<OperationWorkerDTO?> GetWorkerByUserGID(Guid userGID, CancellationToken cancellationToken, string token)
        {
            var operationWorkers = await _operationsGateway.GetOperationWorkers(new PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);

            return operationWorkers.FirstOrDefault(x => x.UserGID == userGID);
        }

        public async Task<GetReportResponse> GenerateEventReport(Guid eventGID, CancellationToken cancellationToken, string token)
        {
            var eventDto = await _operationsGateway.GetEventByGID(eventGID, CancellationToken.None, token);

            if (eventDto == null)
                throw new ServiceException("Event not found");

            var groups = (await _operationsGateway.GetGroups(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)).Where(g => g.EventGID == eventGID);

            var volunteersGroups = groups.Count() != 0
                ? (await _volunteerGroupsGateway.GetVolunteersGroups(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)).Where(vg => groups.Any(g => g.GID == vg.GroupGID))
                : new List<VolunteersGroupsDTO>();

            var volunteers = volunteersGroups.Count() != 0 ?
                (await _volunteerGroupsGateway.GetVolunteers(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)).Where(v => volunteersGroups.Any(vg => vg.VolunteerGID == v.GID))
                : new List<VolunteerDTO>();

            var operationTasks = groups.Count() != 0
                ? (await _operationsGateway.GetOperationTasks(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token)).Where(ot => groups.Any(vg => vg.GID == ot.GroupGID))
                : new List<OperationTaskDTO>();

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
            document.Add(new Paragraph("Звіт про подію")
                .SetFont(boldFont)
                .SetFontSize(20)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            // SECTION: Event Info
            document.Add(new Paragraph("Інформація про подію")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetMarginBottom(5));

            document.Add(CreateLabelValueParagraph("Назва події:", eventDto.Name, font, boldFont));
            document.Add(CreateLabelValueParagraph("Тип події:", eventDetail.EventType, font, boldFont));
            document.Add(CreateLabelValueParagraph("Статус події:", eventDetail.EventStatus, font, boldFont));
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
        public async Task<IEnumerable<OperationWorkerDTO>> GetWorkersByRole(GetOperationWorkersByRoleName request, CancellationToken cancellationToken, string token)
        {
            if (request == null || string.IsNullOrEmpty(request.RoleName))
                throw new ArgumentNullException(nameof(request));

            var users = _authGateway.GetAllUserIdsByRole(request.RoleName, cancellationToken, token);

            var operationWorkers = await _operationsGateway.GetOperationWorkers(new PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);

            return operationWorkers.Where(x => users.Any(u => u.ToLower() == x.UserGID.ToString().ToLower()));
        }

        public async Task<EventDTO> CreateEvent(CreateEventRequest request, string token)
        {
            var user = _authGateway.GetByGID(request.UserGID, CancellationToken.None, token);

            if (user == null)
                throw new ServiceException("User not found");

            var isDispatcher = user.Roles.Any(x => x.Name == SharedConstants.DispatcherRoleName);

            if (!isDispatcher)
                throw new ServiceException("User is not a dispatcher");

            var opeationWorkers = await _operationsGateway.GetOperationWorkers(new PaginationQuery { PageNumber = 0, PageSize = 0 }, CancellationToken.None, token);

            var dispatcher = opeationWorkers.FirstOrDefault(x => x.UserGID == request.UserGID);

            if (dispatcher == null)
                throw new ServiceException("Dispatcher not found");

            var createEventRequestDTO = new CreateEventDTO
            {
                Name = request.Name,
                DispatcherGID = dispatcher.GID,
                CoordinatorGID = request.CoordinatorGID,
                EventStatusGID = SharedConstants.EventStatusCreated,
                EventTypeGID = request.EventTypeGID,
                DistrictGID = request.DistrictGID,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
            };

            var eventDTO = await _operationsGateway.CreateEvent(createEventRequestDTO, token);

            return eventDTO;
        }

        public async Task EventStatusChange(EventStatusChangeRequest request, string token)
        {
            var eventDTO = await _operationsGateway.GetEventByGID(request.EventGID, CancellationToken.None, token);

            if (eventDTO != null && SharedConstants.EventStatuses.Any(e => e.GID == request.EventStatusGID))
            {
                eventDTO.EventStatusGID = request.EventStatusGID;
                eventDTO.Note = request.Note;
                var updatedEvent = _mapper.Map<UpdateEventDTO>(eventDTO);

                await _operationsGateway.UpdateEvent(updatedEvent, token);
            }
        }

        public async Task<GetAllEntitesReponse<DetailEvent>> GetClearEvents(EventPaginationQuery paginationQuery, CancellationToken cancellationToken, string token)
        {
            var result = new GetAllEntitesReponse<DetailEvent>();

            var response = await _operationsGateway.GetSortedEvents(paginationQuery, cancellationToken, token);

            result.TotalCount = response.TotalCount;

            foreach (var item in response.Items)
            {
                result.Items.Add(await ConvertToDetailEvent(item, cancellationToken, token));
            }

            return result;
        }

        private async Task<DetailEvent> ConvertToDetailEvent(EventDTO eventDTO, CancellationToken cancellationToken, string token)
        {
            var dispatcher = await _operationsGateway.GetOperationWorkerByGID(eventDTO.DispatcherGID, cancellationToken, token);
            var coordinator = await _operationsGateway.GetOperationWorkerByGID(eventDTO.CoordinatorGID, cancellationToken, token);

            var clearEvent = new DetailEvent
            {
                GID = eventDTO.GID,
                Name = eventDTO.Name,
                Dispatcher = $"{dispatcher.Name} {dispatcher.Surname} {dispatcher.SecondName}",
                Coordinator = $"{coordinator.Name} {coordinator.Surname} {coordinator.SecondName}",
                EventStatus = SharedConstants.EventStatuses.FirstOrDefault(x => x.GID == eventDTO.EventStatusGID)?.Name!,
                EventType = SharedConstants.EventTypes.FirstOrDefault(x => x.GID == eventDTO.EventTypeGID)?.Name!,
                District = SharedConstants.Districts.FirstOrDefault(x => x.GID == eventDTO.DistrictGID)?.Name!,
                Latitude = eventDTO.Latitude,
                Longitude = eventDTO.Longitude,
                CreatedAt = eventDTO.CreatedAt,
                UpdatedAt = eventDTO.UpdatedAt,
            };
            return clearEvent;
        }
    }
}
