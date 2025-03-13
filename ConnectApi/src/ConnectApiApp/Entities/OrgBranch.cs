#nullable disable
namespace ConnectApiApp.Entities;

public record OrgBranch
{
    public int BranchId { get; set; }

    public int OrganizationId { get; set; }

    public string Name { get; set; }

    public string DbKey { get; set; }

    public string Division { get; set; }

    public bool? Active { get; set; }

    public string ExternalDivisionId { get; set; }
}