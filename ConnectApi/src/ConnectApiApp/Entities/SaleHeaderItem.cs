namespace ConnectApiApp.Entities;

public record SaleHeaderItem
{
    public int SaleId { get; set; }

    public string? CustomerSaleId { get; set; }

    public string? PackOutDate { get; set; }

    public required string Customer { get; set; }

    public int CustomerId { get; set; }

    public string? ExternalCustomerId { get; set; }

    public int LocationId { get; set; }

    public required string Location { get; set; }

    public string? ExternalLocationId { get; set; }

    public int OrderGroupId { get; set; }

    public required string OrderGroup { get; set; }

    public string? ExternalId { get; set; }

    public int ItemQuantityTotal { get; set; }

    public decimal SaleTotal { get; set; }

    public int PackoutAreaId { get; set; }

    public required string PackoutAreaName { get; set; }

    public bool LateOrder { get; set; }

    public string? SaleStatus { get; set; }

    public int SaleStatusId { get; set; }

    public string? Note { get; set; }

    public bool SaleDeleted { get; set; }

    public string? CreatedByUserName { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }
    public int SaleItemId { get; set; }
    public string? CustomerSaleItemId { get; set; }

    public int ProductId { get; set; }

    public string? Sku { get; set; }

    public required string ProductName { get; set; }

    public int QtyOrdered { get; set; }

    public int QuantityPicked { get; set; }

    public decimal UnitPrice { get; set; }

    public bool SaleItemDeleted { get; set; }

    public DateTime? SaleItemCreatedOnUtc { get; set; }

    public DateTime? SaleItemModifiedOnUtc { get; set; }
}
