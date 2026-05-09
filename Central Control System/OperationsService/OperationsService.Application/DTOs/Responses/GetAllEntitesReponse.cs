namespace OperationsService.Application.DTOs.Responses
{
    public class GetAllEntitesReponse<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; } = 0;
    }
}
