using System.Globalization;
using ConnectApiApp.Common.ValidationHelpers;
using FluentValidation;

namespace ConnectApiApp.Features.Sales.GetSales;

public class SalesQueryValidator : AbstractValidator<SalesQuery>
{
    public SalesQueryValidator()
    {
        {
            // Require at least one parameter
            RuleFor(x => new { x.SaleId, x.ModifiedOnUtc })
                .Must(x => x.SaleId.HasValue || !string.IsNullOrEmpty(x.ModifiedOnUtc))
                .WithMessage("Either Sale Id or Last Updated Time (UTC) must be provided");

            // If SaleId is provided, validate it
            When(x => x.SaleId.HasValue, () =>
            {
                RuleFor(x => x.SaleId)
                    .GreaterThan(0)
                    .WithMessage("SaleId must be greater than 0");
            });

            // If UpdatedTimeUtc is provided, validate its format
            When(x => !string.IsNullOrEmpty(x.ModifiedOnUtc), () =>
            {
                RuleFor(x => x.ModifiedOnUtc)
                    .Must(date => date.IsAValidDate("yyyy-MM-dd"))
                    .WithMessage("Last Updated Time (UTC) must be in YYYY-MM-DD format");
            });
        }
    }

}