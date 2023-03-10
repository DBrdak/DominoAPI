namespace DominoAPI.Models.Query
{
    public class FuelNotesQueryParams
    {
        public string? SearchPhrase { get; set; }
        public int PageId { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }
    }
}