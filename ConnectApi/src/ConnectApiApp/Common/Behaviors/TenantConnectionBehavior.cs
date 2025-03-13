using System.Data;
using AutoWrapper.Wrappers;
using ConnectApiApp.Common.Caching;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ConnectApiApp.Common.Behaviors;

public interface IRequireTenantConnection : IBaseRequest
{
    // Marker interface
}

public class TenantContext(string? divisionId = null, IDbConnection? connection = null)
{
    public required string? DivisionId { get; set; } = divisionId;
    public required IDbConnection? Connection { get; set; } = connection;
    public required string DatabaseName { get; set; }
}

//Get the division id from the header
//Get the database name from the cache
//Create a new database connection that can be used in any Mediatr handler

public class CustomerDatabaseBehavior<TRequest, TResponse>(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    TenantContext tenantContext,
    ILogger logger,
    DatabaseNamesCacheService databaseNamesCacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private string _databaseName = null!;
    private const string DivisionHeader = "X-DivisionId";

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var headers = httpContextAccessor.HttpContext?.Request.Headers;
        if (headers == null || !headers.TryGetValue(DivisionHeader, out var divisionId))
            throw new ApiException("Division Id is required!");

        try
        {
            _databaseName = databaseNamesCacheService.GetDbKeyByExternalDivisionId(divisionId.ToString());
            tenantContext.DivisionId = divisionId.ToString();
            tenantContext.Connection = CreateConnection(divisionId.ToString());
            tenantContext.DatabaseName = _databaseName;
            logger.Debug("Created database connection for division {DivisionId}", tenantContext.DivisionId);

            var result = await next();
            logger.Debug("Request handler completed successfully");
            return result;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error occurred while processing request");
            throw;
        }
        finally
        {
            logger.Debug("Cleaning up database connection for division {DivisionId}", tenantContext.DivisionId);
            tenantContext.Connection?.Dispose();
        }
    }

    private IDbConnection CreateConnection(string divisionId)
    {
        var baseConnectionString = configuration.GetConnectionString("ConnectCustomer");

        if (string.IsNullOrWhiteSpace(_databaseName))
        {
            var failure = new ValidationFailure(DivisionHeader, $"Division Is. '{divisionId}' not found!");
            throw new ApiException(new[] { failure });
        }

        var connectionString = new SqlConnectionStringBuilder(baseConnectionString)
        {
            InitialCatalog = _databaseName
        }.ToString();

        var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}