using ConnectApiApp.Common;
using ConnectApiApp.Features.Sales.GetSales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace ConnectApiApp.Features.Sales.PostPickUpdate;

[Route("api/v{version:apiVersion}/[controller]")]
public class PostPickController(ILogger<SaleController> logger) : ApiControllerBase
{
    [Authorize]
    [ApiVersion("1.0")]
    [HttpPost("sales-pick-quantities")]
    [SwaggerOperation(
        Summary = "Sale Pick Quantity Update",
        Description = "Update Sales Pick Quantities",
        OperationId = "Update Sales Pick Quantities",
        Tags = ["Sale"])
    ]
    public async Task<ActionResult<SalesPickQuantitiesCommandResp>> SalesPickQuantities(SalesPickQuantitiesCommand salesPickQuantitiesCommand)
    {
        logger.LogInformation("Sales Pick Quantities");
        return await Mediator.Send(salesPickQuantitiesCommand);
        
    }

}