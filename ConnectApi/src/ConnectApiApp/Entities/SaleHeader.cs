namespace ConnectApiApp.Entities;

public record SaleHeader {
    public int SaleId {get;set;}
    public string? CustomerSaleId {get;set;}
    public string? PackOutDate {get;set;}
    public required string Customer {get;set;}
    public int CustomerId {get;set;}
    public string? ExternalCustomerId {get;set;}
    public int LocationId {get;set;}
    public required string Location {get;set;}
    public string? ExternalLocationId {get;set;}
    public int OrderGroupId {get;set;}
    public required string OrderGroup {get;set;}
    public string? ExternalId {get;set;}
    public int ItemQuantityTotal {get;set;}
    public decimal SaleTotal {get;set;}
    public int PackoutAreaId {get;set;}
    public required string PackoutAreaName {get;set;}
    public bool LateOrder {get;set;}
    public string? SaleStatus {get;set;}
    public int SaleStatusId {get;set;}
    public string? Note {get;set;}
    public bool Deleted {get;set;}
    public string? CreatedByUserName {get;set;}
    public string? CreatedBy {get;set;}
    public DateTime? CreatedOnUtc {get;set;}
    public DateTime? ModifiedOnUtc {get;set;}
}

//select
//s.SaleId,
//s.CustomerSaleId,
//convert(varchar(10), s.PackOutDate, 101) as PackOutDate,
//cc.Name as Customer,
//cc.CustomerCompanyId as CustomerId,
//cc.ExternalCustomerId,
//l.LocationId,
//l.Name as Location,
//l.ExternalLocationId,
//og.OrderGroupId,
//og.Name as OrderGroup,
//og.ExternalId,
//s.ItemQuantityTotal,
//s.SaleTotal,
//pa.PackoutAreaId,
//pa.Name as PackoutAreaName,
//s.LateOrder,
//ss.SaleStatus,
//ss.SaleStatusId,
//s.Note,
//s.Deleted,
//u.UserName as CreatedByUserName,
//u.FirstName + ' ' + u.LastName as CreatedBy,
//coalesce(dbo.UtcToCompany(s.CreatedOnUtc), s.CreatedOnUtc) as CreatedOn,
//coalesce(
//dbo.UtcToCompany(s.ModifiedOnUtc),
//s.ModifiedOnUtc
//) as ModifiedOn
//from
//Sale s
//join AspNetUsers u on s.CreatedByWebUserId = u.Id
//join OrderGroup og on s.OrderGroupId = og.OrderGroupId
//join PackoutArea pa on pa.PackoutAreaId = og.PackoutAreaId
//join Location l on og.LocationId = l.LocationId
//join CustomerCompany cc on cc.CustomerCompanyId = l.CustomerCompanyId
//join SaleStatus ss on ss.SaleStatusId = s.SaleStatusId