using ConnectApiApp.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepoDb;

namespace ConnectApiApp.Common.Caching;

public class DatabaseNamesCacheService(
    IMemoryCache cache,
    IConfiguration configuration,
    ILogger<DatabaseNamesCacheService> logger)
    : BackgroundService
{
    private const string CacheKey = "DatabaseNames";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Initialize RepoDB for SQL Server
        GlobalConfiguration
            .Setup()
            .UseSqlServer();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateCacheAsync();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating database names _cache");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

    private async Task UpdateCacheAsync()
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("ConnectMaster"));

        var dbNames = (await connection.QueryAllAsync<OrgBranch>(fields: [new Field("DbKey"), new Field("ExternalDivisionId")
        ])).ToList();

        // Set _cache with no absolute expiration, but will be refreshed every hour by the background service
        cache.Set(CacheKey, dbNames);
        logger.LogInformation("Database names _cache updated. Count: {Count}", dbNames.Count);
    }

    public string GetDbKeyByExternalDivisionId(string externalDivisionId)
    {
        var cachedOrgBranch = cache.Get<List<OrgBranch>>(CacheKey);
        return cachedOrgBranch?
            .FirstOrDefault(x => string.Equals(x.ExternalDivisionId, externalDivisionId, StringComparison.OrdinalIgnoreCase))?
            .DbKey ?? string.Empty;
    }
}
