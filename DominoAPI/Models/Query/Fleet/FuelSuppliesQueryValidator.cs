using DominoAPI.Entities.Fleet;
using FluentValidation;

namespace DominoAPI.Models.Query.Fleet
{
    public class FuelSuppliesQueryValidator : AbstractValidator<FuelSuppliesQueryParams>
    {
        private readonly int[] _allowedPageSizes = { 10, 15, 20, 25 };
        private readonly string[] _allowedColumnNames = { null, nameof(FuelSupply.DateOfDelivery), nameof(FuelSupply.Price) };

        public FuelSuppliesQueryValidator()
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