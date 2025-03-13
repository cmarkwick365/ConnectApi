using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoDb;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using ConnectApiApp.Entities;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;
using static ConnectApiTests.Extensions;

namespace ConnectApiTests;

/// <summary>
///     this test class came from here:  https://github.com/ChrisKlug/asp-net-core-integration-testing-demo
/// </summary>
public class TestHelper<TEntryPoint>(Auth0Token? token)
    where TEntryPoint : class
{
    private readonly WebApplicationFactory<TEntryPoint> _application = new WebApplicationFactory<TEntryPoint>().WithWebHostBuilder(builder => { });

    public async Task Run(Func<HttpClient, Task> test)
    {
        await test(GetClient());
    }

    public async Task Run(Func<HttpClient, IConfiguration?, Task> test)
    {
        //var application = new WebApplicationFactory<TEntryPoint>().WithWebHostBuilder(builder => { });

        var configuration = _application.Services.GetService<IConfiguration>();

        await test(GetClient(), configuration);
    }

    public async Task Run(Func<HttpClient, IConfiguration?, IServiceProvider, Task> test)
    {
        //var application = new WebApplicationFactory<TEntryPoint>().WithWebHostBuilder(builder => { });

        var configuration = _application.Services.GetService<IConfiguration>();

        await test(GetClient(), configuration,  _application.Services);
    }

    private HttpClient GetClient()
    {
        //var application = new WebApplicationFactory<TEntryPoint>().WithWebHostBuilder(builder => { });
        var client = _application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        return client;
    }    
   
}

public static class Extensions
{
    private static StringContent GenerateTokenRequestBody()
    {
        var client_id = "vqPPsUgEr4KN5O83BV0dzt4xVvHprdZx";
        var client_secret = "jTs5Qqz9h-atIFEcZhUfCP_S6svSZgz_qTXOFvisEPF8WscNfq5W8fSMAD1RGvon";
        var audience = "https://lightspeed.com";
        var grant_type = "client_credentials";

        var body = JsonSerializer.Serialize(new { client_id, client_secret, audience, grant_type  },
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return new StringContent(body, Encoding.UTF8, "application/json");
    }
    
    public class Auth0Token
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
        
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        
        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }
    }
    
    public static async Task<Auth0Token?> GetBearerToken()
    {
        using var client = new HttpClient();
        var result = await client.PostAsync("https://dev-lightspeed365.us.auth0.com/oauth/token",
            GenerateTokenRequestBody());

        result.EnsureSuccessStatusCode();

        var response = await result.Content.ReadAsStringAsync();

        return DeserializeResponse<Auth0Token>(response);
    }
    
    public static T? DeserializeResponse<T>(string response)
    {
        return JsonSerializer.Deserialize<T>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    
    public static int ExecuteNonQuery(this HttpClient client, string query, IConfiguration? configuration)
    {
        if (configuration == null) return 0;
        using var connection = new SqlConnection(configuration.GetConnectionString("ConnectTestCustomer"));
        return connection.ExecuteNonQuery(query);
    }

    public static int QueryInt(this HttpClient client, string query, IConfiguration? configuration)
    {
        if (configuration == null) return 0;
        using var connection = new SqlConnection(configuration.GetConnectionString("ConnectTestCustomer"));
        return connection.ExecuteScalar<int>(query);
    }

    public static IEnumerable<T>? ExecuteQuery<T>(this HttpClient client, string query, IConfiguration? configuration)  where T : class
    {
        if (configuration == null) return null;
        using var connection = new SqlConnection(configuration.GetConnectionString("ConnectTestCustomer"));

        return connection.ExecuteQuery<T>(query);
    }

  
    public static async Task<T1?> QueryFirstAsync<T1>(this HttpClient client, Expression<Func<T1, bool>>? where, IConfiguration? configuration)  where T1 : class 
    {
        if (configuration == null) return null;
        await using var connection = new SqlConnection(configuration.GetConnectionString("ConnectTestCustomer"));
        return (await connection.QueryAsync(where, hints: SqlServerTableHints.NoLock)).FirstOrDefault();
    }

    public static int BulkInsert<T>(this HttpClient client, IEnumerable<T> collection, IConfiguration? configuration)
        where T : class
    {
        if (configuration == null) return 0;
        using var connection = new SqlConnection(configuration.GetConnectionString("ConnectTestCustomer"));
        return connection.BulkInsert(collection);
    }

    public static int BulkDelete<T>(this HttpClient client, IEnumerable<T> collection, IConfiguration? configuration, Expression<Func<T, object>>? expression = null)
        where T : class
    {
        if (configuration == null) return 0;
        using var connection = new SqlConnection(configuration.GetConnectionString("ConnectTestCustomer"));

        return connection.BulkDelete(collection, qualifiers: expression);
    }
}