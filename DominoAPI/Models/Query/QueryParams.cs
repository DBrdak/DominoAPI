namespace DominoAPI.Models.Query
{
    public class QueryParams
    {
        public string? SearchPhrase { get; set; }
        public int PageId { get; set; }
        public int PageSize { get; set; }
    }
}