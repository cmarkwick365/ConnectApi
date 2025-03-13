using ConnectApiApp.Common.Behaviors;
using System.Data;
using ConnectApiApp.Entities;
using RepoDb;

namespace ConnectApiApp.Common.ValidationHelpers;

public static class Extensions
{
    public static bool IsAValidDate(this string? date, string format)
    {
        return !string.IsNullOrWhiteSpace(date) && DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out _);
    }

    public static async Task<bool> SaleIdExists(this ISaleItemValidator saleItemValidator,
        IDbConnection? tenantContextConnection)
    {
        return await tenantContextConnection.ExistsAsync<SaleItemView>(x => x.SaleId == saleItemValidator.SaleId);
    }

    public static async Task<bool> SaleItemIdExists(this ISaleItemValidator saleItemValidator,
        IDbConnection? tenantContextConnection)
    {
        return await tenantContextConnection.ExistsAsync<SaleItemView>(x => x.SaleItemId == saleItemValidator.SaleItemId);
    }

    public static async Task<bool> ProductIdExists(this ISaleItemValidator saleItemValidator,
        IDbConnection? tenantContextConnection)
    {
        return await tenantContextConnection.ExistsAsync<SaleItemView>(x => x.ProductId == saleItemValidator.ProductId);
    }


}

public interface ISaleItemValidator
{
    public int SaleId { get; set; }
    public int SaleItemId { get; set; }
    public int ProductId { get; set; }
}