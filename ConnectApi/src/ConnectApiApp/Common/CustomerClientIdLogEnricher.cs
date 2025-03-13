using ConnectApiApp.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepoDb;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ConnectApiApp.Common;

public class CustomerClientIdLogEnricher(ConfigurationManager builderConfiguration) : ILogEventEnricher
{
    private readonly string _connectionString = new(builderConfiguration.GetConnectionString("ConnectMaster"));
    private string? _clientId;
    private const string NotFound = "Not Found";


    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (string.IsNullOrEmpty(_clientId)) GetCustomerClientId();

        var enrichProperty = propertyFactory
            .CreateProperty(
                "ClientId",
                _clientId);

        logEvent.AddOrUpdateProperty(enrichProperty);
    }

    private void GetCustomerClientId()
    {
        GlobalConfiguration
            .Setup()
            .UseSqlServer();

        try
        {
            //using var connection = new SqlConnection(_connectionString);
            //var configuration = connection
            //    .Query<Configuration>(e => e.ConfigName == "ConnectMaster" && e.Element == "clientID")
            //    .FirstOrDefault();

            //_clientId = configuration?.Value ?? NotFound;
            _clientId = "999";

        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Error getting client Id.");
            _clientId = NotFound;
        }
    }
}