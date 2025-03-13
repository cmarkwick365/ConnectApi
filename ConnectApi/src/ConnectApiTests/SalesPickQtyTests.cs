using System.Net.Http.Json;
using ConnectApiApp.Dto;
using ConnectApiApp.Entities;
using ConnectApiApp.Features.Sales.PostPickUpdate;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConnectApiTests;

public class SalesPickQtyTests(ITestOutputHelper testOutputHelper)
{
    private readonly Extensions.Auth0Token? _token = Extensions.GetBearerToken().Result;


    [Fact]
    public Task Can_Update_Pick_Qty()
    {
        return new TestHelper<Program>(_token)
            .Run(async (client, config) =>
            {
                client.DefaultRequestHeaders.Add("X-DivisionId", "8B");

                //get some test data
                var saleItemView = client.ExecuteQuery<SaleItemView>("SELECT TOP 1 * FROM SaleItemView", config)!.First();

                //reset quantity
                client.ExecuteNonQuery($"UPDATE SaleItem Set Quantity = 1 WHERE SaleItemId = {saleItemView.SaleItemId}", config);

                //set quantity to 99
                var salesPickQuantitiesCommand = new SalesPickQuantitiesCommand
                {
                    SalesPickQuantities = new List<SalesPickQuantitiesDto>
                    {
                        new()
                        {
                            ProductId = saleItemView.ProductId, Quantity = 99, SaleId = saleItemView.SaleId,
                            SaleItemId = saleItemView.SaleItemId
                        }
                    }
                };

                var response =
                    await client.PostAsJsonAsync("/api/v1/postpick/sales-pick-quantities", salesPickQuantitiesCommand);
                response.EnsureSuccessStatusCode();

                var result = client.QueryInt($"SELECT Quantity FROM SaleItemView WHERE SaleItemId = {saleItemView.SaleItemId}", config);
                result.Should().Be(99);
            });
    }


    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public T Result { get; set; } = default!;
    }
}