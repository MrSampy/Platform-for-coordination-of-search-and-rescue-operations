namespace Gateway.Domain.Services.Interfaces
{
    public interface IEfficiencyCalculationService
    {
        Task<double> CalculateEfficiency(string eventType, int volunteersInEventCount, int completedTAsksCount, int allTasksCount, decimal allAvaliableResources, decimal allRequiredResources, CancellationToken cancellationToken, string token);
        Task<double> CalculateAverageVolunteersCountInEventType(string eventType, CancellationToken cancellationToken, string token);
    }
}
