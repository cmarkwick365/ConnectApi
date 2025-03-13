using ConnectApiApp.Common.Behaviors;
using ConnectApiApp.Common.ValidationHelpers;
using FluentValidation;

namespace ConnectApiApp.Features.Sales.PostPickUpdate;

public class SalesPickQuantitiesValidator : AbstractValidator<SalesPickQuantitiesCommand>
{
    public SalesPickQuantitiesValidator(TenantContext tenantContext)
    {
        RuleForEach(x => x.SalesPickQuantities).ChildRules(spq =>
        {
            spq.RuleFor(x => x.ProductId).NotNull().GreaterThanOrEqualTo(0).WithMessage("Product ID is required");
            spq.RuleFor(x => x.SaleId).NotNull().WithMessage("Sale ID is required");
            spq.RuleFor(x => x.SaleItemId).NotNull().WithMessage("Sale Item ID is required");
            spq.RuleFor(x => x.Quantity).NotNull().GreaterThanOrEqualTo(0).WithMessage("Quantity is required");

            // Only validate SaleId exists if it's greater than 0
            spq.RuleFor(x => x)
                .MustAsync(async (s, _) => s.SaleItemId == 0 || await s.SaleIdExists(tenantContext.Connection))
                .WithMessage("Sale ID does not exist.");

            // Only validate SaleItemId exists if it's greater than 0
            spq.RuleFor(x => x)
                .MustAsync(async (s, _) => s.SaleItemId == 0 || await s.SaleItemIdExists(tenantContext.Connection))
                .WithMessage("Sale Item ID does not exist.");

            spq.RuleFor(x => x).MustAsync(async (s, _) => await s.ProductIdExists(tenantContext.Connection))
                .WithMessage("Product ID does not exist.");
        });
    }
}