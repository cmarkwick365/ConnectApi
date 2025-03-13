using AutoMapper;
using AutoWrapper.Wrappers;
using ConnectApiApp.Common.Behaviors;
using LsConnectClient;
using LsConnectService;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace ConnectApiApp.Features.Sales.PostPickUpdate;

public class SalesPickQuantitiesCreateHandler(
    ILogger logger,
    IWcfClient wcfClient,
    IMapper mapper,
    TenantContext tenantContext)
    : IRequestHandler<SalesPickQuantitiesCommand, SalesPickQuantitiesCommandResp>
{
    public async Task<SalesPickQuantitiesCommandResp> Handle(SalesPickQuantitiesCommand request,
        CancellationToken cancellationToken)
    {
        foreach (var spq in request.SalesPickQuantities)
        {
            var orderItemDefinition = mapper.Map<OrderItemDefinition>(spq);
            var success = await wcfClient.ConnectServerClient.AddUpdateOrderItemAsync(tenantContext.DatabaseName,
                orderItemDefinition, spq.SaleId, "");

            if (success) continue;

            logger.Error(
                "Unable to update Sales Picked Quantities. The error will most likely be logged in Connect Event Log");
            throw new ApiException(
                $"Update Sales Picked Quantities Failed.  Sale Id: {spq.SaleId} Sale Item Id: {spq.SaleItemId} Contact Support!",
                StatusCodes.Status500InternalServerError);
        }

        return new SalesPickQuantitiesCommandResp { Success = true };
    }
}