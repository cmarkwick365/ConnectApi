using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectApiTests;

/// <summary>
/// this test class came from here:  https://github.com/ChrisKlug/asp-net-core-integration-testing-demo
/// </summary>
public abstract class TestBase
{
    protected async Task RunTest(Func<HttpClient, Task> test, string? environmentName = null,
        IDictionary<string, string>? configuration = null)
    {
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
          

            if (environmentName != null)
                builder.UseEnvironment(environmentName);

            if (configuration != null)
                builder.ConfigureAppConfiguration((ctx, config) =>
                {
                    config.AddInMemoryCollection(configuration!);
                });

            builder.ConfigureTestServices(ConfigureTestServices);

        });

        using var services = application.Services.CreateScope();
        var client = application.CreateClient();

        await test(client);
    }



    protected abstract void ConfigureTestServices(IServiceCollection services);
}