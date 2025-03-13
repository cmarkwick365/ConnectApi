using MediatR;
using ConnectApiApp.Dto;

namespace ConnectApiApp.Features.Sales.PostPickUpdate
{
    public class SalesPickQuantitiesCommandResp
    {
        public bool Success { get; set; }
    }
    public class SalesPickQuantitiesCommand : IRequest<SalesPickQuantitiesCommandResp>
    {
        public required IEnumerable<SalesPickQuantitiesDto> SalesPickQuantities { get; set; }
    }
}
