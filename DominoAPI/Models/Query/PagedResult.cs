namespace DominoAPI.Models.Query
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int TotalPages { get; set; }
        public int ResultsFrom { get; set; }

        public int ResultsTo { get; set; }

        public int ResultsCount { get; set; }

        public PagedResult(IEnumerable<T> results, int resultsCount, int pageSize, int pageId)
        {
            Results = results;
            ResultsCount = resultsCount;
            ResultsFrom = pageSize * (pageId - 1) + 1;
            ResultsTo = ResultsFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(resultsCount / (double)pageSize);
        }
    }
}