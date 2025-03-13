namespace ConnectApiApp.Entities;

public record SaleItemView
{
    public int SaleItemId { get; set; }

    public int SaleId { get; set; }

    public int ProductId { get; set; }

    public string? Sku { get; set; }

    public required string ProductName { get; set; }

    public decimal ProductPrice { get; set; }

    public int Quantity { get; set; }

    public decimal? ExtendedPrice { get; set; }

    public decimal? Weight { get; set; }

    public decimal? WeightInOz { get; set; }

    public decimal? ExtendedWeight { get; set; }

    public decimal? ExtendedWeightInOz { get; set; }

    public string? MeasurementUnitAbbrev { get; set; }

    public int? UnitsOfMeasureId { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public string? ModifiedByWebUserId { get; set; }

    public int QtyOrdered { get; set; }
}

