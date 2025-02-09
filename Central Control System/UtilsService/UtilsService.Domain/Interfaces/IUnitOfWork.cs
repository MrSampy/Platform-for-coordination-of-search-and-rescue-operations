namespace UtilsService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IDistrictRepository DistrictRepository { get; }
        IMeasurementUnitRepository MeasurementUnitRepository { get; }
        IResourceRepository ResourceRepository { get; }
        IResourceMeasurementUnitRepository ResourceMeasurementUnitRepository { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
