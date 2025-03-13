using AutoMapper;
using ConnectApiApp.Common.Mappings;
using ConnectApiApp.Common.ValidationHelpers;

namespace ConnectApiApp.Dto;

public record SalesPickQuantitiesDto : IMapFrom<LsConnectService.OrderItemDefinition>, IMapFrom<SalesPickQuantitiesDto>, ISaleItemValidator
{
    public int SaleId { get; set; }
    public int SaleItemId { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }

public void Mapping(Profile profile)
{
    profile.CreateMap<SalesPickQuantitiesDto, LsConnectService.OrderItemDefinition>()
        .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.SaleItemId))
        ;
   
}

}
