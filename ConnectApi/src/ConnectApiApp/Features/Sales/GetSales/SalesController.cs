using AutoWrapper.Wrappers;
using ConnectApiApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace ConnectApiApp.Features.Sales.GetSales;

[Route("api/v{version:apiVersion}/[controller]")]
public class SaleController(ILogger<SaleController> logger) : ApiControllerBase
{
    [Authorize]
    [ApiVersion("1.0")]
    [HttpGet("sales")]
    [SwaggerOperation(
        Summary = "Get Sales",
        Description = "Get Sales By Sale Id. and Last Updated Time (UTC)",
        OperationId = "Get Sales",
        Tags = ["Sale"])
    ]
    public async Task<ApiResponse> Sales(int? saleId, string? updatedTimeUtc)
    {
        logger.LogInformation("Get Sales");
        var resp = await Mediator.Send(new SalesQuery { SaleId = saleId, ModifiedOnUtc = updatedTimeUtc });
        var statusCode = resp.Sales != null && resp.Sales.Any() ? StatusCodes.Status200OK : StatusCodes.Status404NotFound;

        return new ApiResponse(resp.Sales, statusCode);
    }

}