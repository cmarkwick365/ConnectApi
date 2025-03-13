using ConnectApiApp.Dto;
using MediatR;

namespace ConnectApiApp.Features.Sales.GetSales;

public class SalesQuery : IRequest<SalesQueryResp>
{
    public int? SaleId { get; set; }
    public string? ModifiedOnUtc { get; set; }
}

public class SalesQueryResp
{
    public required IEnumerable<SaleHeaderItemDto>? Sales { get; set; }
}