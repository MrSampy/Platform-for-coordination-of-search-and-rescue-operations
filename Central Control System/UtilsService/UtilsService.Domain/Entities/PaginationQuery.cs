namespace UtilsService.Domain.Entities
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public bool IsValid() { return PageNumber >= 0 && PageSize >= 0; }
        public bool GetAll() { return PageNumber == 0 && PageSize == 0; }
    }
}
