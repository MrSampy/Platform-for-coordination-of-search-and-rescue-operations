using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;

namespace Gateway.Infrastructure.Services.Services
{
    public class EfficiencyCalculationService : IEfficiencyCalculationService
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IVolunteersGateway _volunteersGateway;
        public EfficiencyCalculationService(IOperationsGateway operationsGateway, IVolunteersGateway volunteersGateway)
        {
            _operationsGateway = operationsGateway;
            _volunteersGateway = volunteersGateway;
        }


        public async Task<double> CalculateEfficiency(
            string eventType,
            int volunteersInEventCount,
            int completedTasksCount,
            int allTasksCount,
            decimal allAvaliableResources,
            decimal allRequiredResources,
            CancellationToken cancellationToken, string token)
        {
            var typeScore = eventType switch
            {
                "Пошук" => 3,
                "Евакуація" => 2,
                "Логістика" => 1,
                _ => 1
            };

            double V_max = await CalculateAverageVolunteersCountInEventType(eventType, cancellationToken, token);
            double w1 = 0.15; // Тип операції
            double w2 = 0.2;  // Волонтери
            double w3 = 0.4;  // Завдання
            double w4 = 0.25; // Ресурси

            // Normalized values
            double type_norm = typeScore / 3.0;
            double V_norm = Math.Min((double)volunteersInEventCount / V_max, 1.0);
            double C_norm = allTasksCount > 0 ? (double)completedTasksCount / allTasksCount : 0;
            double R_norm = allRequiredResources > 0 ? (double)allAvaliableResources / (double)allRequiredResources : 0;
            R_norm = Math.Min(R_norm, 1.0);

            double efficiency = (
                  w1 * type_norm
                + w2 * V_norm
                + w3 * C_norm
                + w4 * R_norm
            ) * 100;

            return Math.Round(efficiency, 2);
        }

        public async Task<double> CalculateAverageVolunteersCountInEventType(string eventType, CancellationToken cancellationToken, string token)
        {
            var count = SharedConstants.DEFAULT_VOLUNTEER_COUNT;

            var eventTypeGID = SharedConstants.EventTypes
                .FirstOrDefault(x => x.Name == eventType)?.GID;

            if (eventTypeGID == null)
            {
                return count;
            }

            var events = await _operationsGateway.GetSortedEvents(new DTO.DTOs.Operations.Request.EventPaginationQuery { PageNumber = 0, PageSize = 0, EventTypeGID = eventTypeGID }, cancellationToken, token);

            if (events == null || !events.Items.Any())
            {
                return count;
            }

            var eventGIDs = events.Items.Select(x => x.GID).ToList();

            var volunteersInEvents = await _volunteersGateway.GetVolunteersEvents(new DTO.DTOs.Common.PaginationQuery { PageNumber = 0, PageSize = 0 }, cancellationToken, token);

            var volunteersCount = volunteersInEvents
                .Where(x => eventGIDs.Contains(x.EventGID))
                .GroupBy(x => x.EventGID)
                .Select(g => new { VolunteersCount = g.Count() })
                .Average(v => v.VolunteersCount);


            return volunteersCount;
        }
    }
}
