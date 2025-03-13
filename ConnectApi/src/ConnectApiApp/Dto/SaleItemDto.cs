using AutoMapper;
using ConnectApiApp.Common.Mappings;
using ConnectApiApp.Entities;

namespace ConnectApiApp.Dto;

public record SaleItemDto  : IMapFrom<SaleHeaderItem>
{ 
    public int SaleItemId { get; set; }
    public string? CustomerSaleItemId { get; set; }

    public int ProductId { get; set; }

    public string? Sku { get; set; }

    public required string ProductName { get; set; }

    public int QtyOrdered { get; set; }

    public int QuantityPicked { get; set; }

    public decimal UnitPrice { get; set; }

    public bool SaleItemDeleted { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SaleHeaderItem, SaleItemDto>()
            .ForMember(dest => dest.ModifiedOnUtc, opt => opt.MapFrom(src => src.SaleItemModifiedOnUtc))
            .ForMember(dest => dest.CreatedOnUtc, opt => opt.MapFrom(src => src.SaleItemCreatedOnUtc))
            ;
    }
}