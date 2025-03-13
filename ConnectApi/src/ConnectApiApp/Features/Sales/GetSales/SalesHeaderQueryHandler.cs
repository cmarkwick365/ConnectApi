using AutoMapper;
using ConnectApiApp.Common;
using ConnectApiApp.Common.Behaviors;
using ConnectApiApp.Dto;
using ConnectApiApp.Entities;
using MediatR;
using RepoDb;
using Serilog;

namespace ConnectApiApp.Features.Sales.GetSales;

public class SalesHeaderQueryHandler(ILogger logger, IMapper mapper, TenantContext tenantContext)
    : IRequestHandler<SalesQuery, SalesQueryResp>, IRequireTenantConnection
{
    public async Task<SalesQueryResp> Handle(SalesQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<SaleHeaderItemDto>? saleHeaderItemDto;
        try
        {
            var result =
                await tenantContext.Connection.ExecuteQueryAsync<SaleHeaderItem>(SqlQueries.SalesHeaderItemQuery,
                    new { request.ModifiedOnUtc, request.SaleId },
                    cancellationToken: cancellationToken);

            saleHeaderItemDto = result.GroupBy(x => x.SaleId)
                    .Select(y =>
                    {
                        var headerItemDto = y.Select(mapper.Map<SaleHeaderItemDto>).First();
                        headerItemDto.SaleItems = y.Select(mapper.Map<SaleItemDto>);
                        return headerItemDto;
                    })
                ;
        }
        catch (Exception e)
        {
            logger.Error("SalesHeaderItemQuery failed.");
            throw new ApiResponseException(logger, e, "SalesHeaderItemQuery failed.");
        }


        return new SalesQueryResp
        {
            Sales = saleHeaderItemDto
        };
    }
}