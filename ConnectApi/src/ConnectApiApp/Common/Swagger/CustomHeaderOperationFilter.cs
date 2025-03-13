using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ConnectApiApp.Common.Swagger;

public class CustomHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-DivisionId",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string"
            },
            Description = "External Division"
        });
    }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireCustomHeaderAttribute : Attribute
{
}