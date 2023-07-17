namespace Sales.Shared.DTOs
{
    public class PaginationDTO
    {
        public int id { get; set; }

        public int Page { get; set; } = 1;

        public int RecordsNumber { get; set; } = 10;
    }
}
