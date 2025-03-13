using AutoWrapper;
using ConnectApiApp;
using ConnectApiApp.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using RepoDb;
using Serilog;
using OpenTelemetry.Metrics;
using OpenTelemetry;
using OpenTelemetry.Exporter.Instana;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ConnectApiApp.Common.Swagger;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
});

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options => options.AddDefaultPolicy(
    policy => policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.With(new CustomerClientIdLogEnricher(builder.Configuration))
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

builder.Services.AddProblemDetails();

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddHealthChecks();
builder.Services.AddOpenTelemetry().WithMetrics(metrics => metrics
    .AddAspNetCoreInstrumentation()
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
    .AddMeter("System.Net.Http")
);

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("ConnectApiApi")
    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName: "ConnectApiApi", serviceVersion: "1.0.0"))
    .AddConsoleExporter()
    .AddInstanaExporter()
    .Build();

builder.Services.AddHttpContextAccessor();
Log.Logger.Information($"***** Authority https://{builder.Configuration["Auth0:Domain"]}");
Log.Logger.Information($"***** Audience {builder.Configuration["Auth0:Audience"]}");
Log.Logger.Information($"***** Domain {builder.Configuration["Auth0:Domain"]}");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.TokenValidationParameters = 
            new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidAudience = builder.Configuration["Auth0:Audience"],
                ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
            };
    });
GlobalConfiguration
    .Setup()
    .UseSqlServer();
var app = builder.Build();
app.UseCors();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}
app.UseApiResponseAndExceptionWrapper();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); 
var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerFeatures(builder.Configuration, apiVersionProvider, app.Environment);
app.UseSerilogRequestLogging();

app.Run();

//used for integration testing don't remove
public partial class Program
{

}

