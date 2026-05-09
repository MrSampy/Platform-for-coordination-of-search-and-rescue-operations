namespace Gateway.DTO.DTOs.Common
{
    public class GetAllEntitesReponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; } = 0;
    }
}
