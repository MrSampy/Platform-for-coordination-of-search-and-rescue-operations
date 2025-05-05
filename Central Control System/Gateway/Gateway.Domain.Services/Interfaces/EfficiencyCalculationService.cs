namespace Gateway.Domain.Services.Interfaces
{
    public interface IEfficiencyCalculationService
    {
        double CalculateEfficiency(string eventType, int volunteersInEventCount, int completedTAsksCount, int allTasksCount, decimal allAvaliableResources, decimal allRequiredResources);
    }
}
