namespace ConnectApiApp.Features.Sales.GetSales;

internal static class SqlQueries
{
    public static readonly string SalesHeaderItemQuery = @"SELECT
                                                      s.SaleId,
                                                      s.CustomerSaleId,
                                                      CONVERT(varchar(10), s.PackOutDate, 101) AS PackOutDate,
                                                      cc.Name AS Customer,
                                                      cc.CustomerCompanyId AS CustomerId,
                                                      cc.ExternalCustomerId,
                                                      l.LocationId,
                                                      l.Name AS Location,
                                                      l.ExternalLocationId,
                                                      og.OrderGroupId,
                                                      og.Name AS OrderGroup,
                                                      og.ExternalId,
                                                      s.ItemQuantityTotal,
                                                      s.SaleTotal,
                                                      pa.PackoutAreaId,
                                                      pa.Name AS PackoutAreaName,
                                                      s.LateOrder,
                                                      ss.SaleStatus,
                                                      ss.SaleStatusId,
                                                      s.Note,
                                                      s.Deleted,
                                                      u.UserName AS CreatedByUserName,
                                                      u.FirstName + ' ' + u.LastName AS CreatedBy,
                                                      COALESCE(dbo.UtcToCompany(s.CreatedOnUtc), s.CreatedOnUtc) AS CreatedOnUtc,
                                                      COALESCE(
                                                        dbo.UtcToCompany(s.ModifiedOnUtc),
                                                        s.ModifiedOnUtc
                                                      ) AS ModifiedOnUtc,
                                                      si.SaleItemId,
                                                      si.CustomerSaleItemId,
                                                      si.ProductId,
                                                      p.Sku,
                                                      p.Name AS ProductName,
                                                      si.QtyOrdered,
                                                      si.Quantity AS QuantityPicked,
                                                      si.UnitPrice,
                                                      si.Deleted,
                                                      COALESCE(dbo.UtcToCompany(s.CreatedOnUtc), si.CreatedOnUtc) AS SaleItemCreatedOnUtc,
                                                      COALESCE(
                                                        dbo.UtcToCompany(si.ModifiedOnUtc),
                                                        si.ModifiedOnUtc
                                                      ) AS SaleItemModifiedOnUtc
                                                    FROM
                                                      Sale s WITH(NOLOCk)
                                                      JOIN AspNetUsers u WITH(NOLOCk) ON s.CreatedByWebUserId = u.Id
                                                      JOIN OrderGroup og WITH(NOLOCk) ON s.OrderGroupId = og.OrderGroupId
                                                      JOIN PackoutArea pa WITH(NOLOCk) ON pa.PackoutAreaId = og.PackoutAreaId
                                                      JOIN Location l WITH(NOLOCk) ON og.LocationId = l.LocationId
                                                      JOIN CustomerCompany cc WITH(NOLOCk) ON cc.CustomerCompanyId = l.CustomerCompanyId
                                                      JOIN SaleStatus ss WITH(NOLOCk) ON ss.SaleStatusId = s.SaleStatusId
                                                      JOIN SaleItem si WITH(NOLOCK) ON si.SaleId = s.SaleId
                                                      JOIN Product p WITH(NOLOCK) ON p.ProductId = si.ProductId
                                                    WHERE s.SaleId = ISNULL(@SaleId, s.SaleId) 
                                                          AND Cast(s.ModifiedOnUtc as Date) = ISNULL(@ModifiedOnUtc, Cast(s.ModifiedOnUtc as Date))
                                                          AND s.ModifiedOnUtc IS NOT NULL
";
}