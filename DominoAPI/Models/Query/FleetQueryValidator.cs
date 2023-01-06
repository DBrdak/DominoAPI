using FluentValidation;

namespace DominoAPI.Models.Query
{
    public class FleetQueryValidator : AbstractValidator<QueryParams>
    {
        private int[] _allowedPageSizes = { 5, 10, 15, 20, 25 };

        public FleetQueryValidator()
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
        }
    }
}