using DominoAPI.Entities.Shops;
using FluentValidation;

namespace DominoAPI.Models.Query
{
    public class SalesQueryValidator : AbstractValidator<SalesQueryParams>
    {
        private readonly int[] _allowedPageSizes = { 10, 15, 20, 25 };
        private readonly string[] _allowedColumnNames = { null, nameof(Sale.Date), nameof(Sale.SaleAmount), nameof(Sale.Bills) };

        public SalesQueryValidator()
        {
            RuleFor(p => p.PageId).GreaterThanOrEqualTo(1);
            RuleFor(p => p.PageSize).Custom((value, context) =>
            {
                if (!_allowedPageSizes.Contains(value))
                {
                    context.AddFailure
                        ("PageSize", $"Page size must be one of: [{String.Join(",", _allowedPageSizes)}]");
                }
            });
            RuleFor(p => p.SortBy).Custom((value, context) =>
            {
                if (!_allowedColumnNames.Contains(value))
                {
                    context.AddFailure
                    ("ColumnName",
                        $"Content can be sortet only by columns: [{String.Join(",", _allowedColumnNames)}]");
                }
            });
        }
    }
}