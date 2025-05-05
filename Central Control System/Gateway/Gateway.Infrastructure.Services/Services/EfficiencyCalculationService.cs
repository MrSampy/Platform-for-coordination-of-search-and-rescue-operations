using Gateway.Domain.Services.Interfaces;

namespace Gateway.Infrastructure.Services.Services
{
    public class EfficiencyCalculationService : IEfficiencyCalculationService
    {
        public double CalculateEfficiency(
            string eventType,
            int volunteersInEventCount,
            int completedTasksCount,
            int allTasksCount,
            decimal allAvaliableResources,
            decimal allRequiredResources)
        {
            var typeScore = eventType switch
            {
                "Пошук" => 3,
                "Евакуація" => 2,
                "Логістика" => 1,
                _ => 1
            };

            double V_max = 20;
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

    }
}
