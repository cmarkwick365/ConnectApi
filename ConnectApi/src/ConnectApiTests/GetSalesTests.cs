using System.Text.Json;
using ConnectApiApp.Dto;
using FluentAssertions;
using Xunit.Abstractions;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ConnectApiTests;

public class GetSalesTests(ITestOutputHelper testOutputHelper)
{
    private readonly Extensions.Auth0Token? _token = Extensions.GetBearerToken().Result;

   

    [Fact]
    public Task Can_Get_Sales_By_Sale_Id_And_Last_Update_Time()
    {
        return new TestHelper<Program>(_token)
            .Run(async (client, _, _) =>
            {
                client.DefaultRequestHeaders.Add("X-DivisionId","8B");
                var response = await client.GetAsync("/api/v1/sale/sales?saleId=179174&&updatedTimeUtc=2024-10-31");
                response.EnsureSuccessStatusCode();
                var sales = (await ApiResponseHandler.DeserializeApiResponse<IEnumerable<SaleHeaderItemDto>>(response)).ToList();
                sales.Any().Should().BeTrue();

            });
    }
    
    [Fact]
    public Task Can_Get_Sales_By_Sale_Id()
    {
        return new TestHelper<Program>(_token)
            .Run(async (client, _, _) =>
            {
                client.DefaultRequestHeaders.Add("X-DivisionId","8B");
                var response = await client.GetAsync("/api/v1/sale/sales?saleId=179174");
                response.EnsureSuccessStatusCode();
                var sales = (await ApiResponseHandler.DeserializeApiResponse<IEnumerable<SaleHeaderItemDto>>(response)).ToList();
                sales.Any().Should().BeTrue();
                sales.First().SaleItems!.Any().Should().BeTrue();
            });
    }

    [Fact]
    public Task Can_Get_Sales_By_Last_Update_Time()
    {
        return new TestHelper<Program>(_token)
            .Run(async (client, _, _) =>
            {
                client.DefaultRequestHeaders.Add("X-DivisionId","8B");
                var response = await client.GetAsync("/api/v1/sale/sales?updatedTimeUtc=2023-06-30");
                response.EnsureSuccessStatusCode();
                var sales = (await ApiResponseHandler.DeserializeApiResponse<IEnumerable<SaleHeaderItemDto>>(response)).ToList();
                sales.Any().Should().BeTrue();
                sales.First().SaleItems!.Any().Should().BeTrue();
            });
    }


    public static class ApiResponseHandler
    {
        public static async Task<T> DeserializeApiResponse<T>(HttpResponseMessage response) where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API request failed with status code: {response.StatusCode}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
        
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(jsonString, options);
        
            if (apiResponse?.Result == null)
            {
                throw new JsonException("Failed to deserialize API response or result was null");
            }

            return apiResponse.Result;
        }
    }

    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public T Result { get; set; } = default!;
    }
}

