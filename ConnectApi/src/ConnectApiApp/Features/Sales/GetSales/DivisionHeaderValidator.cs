using ConnectApiApp.Common.Behaviors;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ConnectApiApp.Features.Sales.GetSales;

public class TenantConnectionValidator : AbstractValidator<IRequireTenantConnection>
{
    private const string DivisionHeader = "X-DivisionId";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantConnectionValidator(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        RuleFor(x => x)
            .CustomAsync(ValidateDivisionHeader);
    }

    private Task ValidateDivisionHeader(IRequireTenantConnection request, ValidationContext<IRequireTenantConnection> context, CancellationToken cancellationToken)
    {
        var headers = _httpContextAccessor.HttpContext?.Request.Headers;

        if (headers == null || !headers.TryGetValue(DivisionHeader, out _))
        {
            context.AddFailure(DivisionHeader, "Division ID header is required");
        }

        return Task.CompletedTask;
    }
}