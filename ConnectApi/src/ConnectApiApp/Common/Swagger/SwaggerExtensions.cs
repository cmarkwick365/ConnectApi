using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ConnectApiApp.Common.Swagger;
public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerFeatures(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        services.AddSwaggerGen(cfg =>
        {
            var provider = services.BuildServiceProvider();
            var service = provider.GetRequiredService<IApiVersionDescriptionProvider>();
            service.ApiVersionDescriptions.ToList().ForEach(apiVersionDescription =>
            {
                cfg.SwaggerDoc(apiVersionDescription.GroupName, new OpenApiInfo
                {
                    Title = "LightSpeed Connect API",
                    Version = apiVersionDescription.ApiVersion.ToString(),
                    Description = $"<strong>Release: {assemblyVersion}</strong>"
                });
                cfg.OperationFilter<CustomHeaderOperationFilter>();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerFeatures(this IApplicationBuilder app, IConfiguration config,
        IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
    {
        //var clientId = config.GetValue<string>("Auth0:ClientId");
        var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(cfg =>
        {
            cfg.InjectStylesheet("../swagger-custom/swagger-custom-styles.css");
            cfg.InjectJavascript("../swagger-custom/swagger-custom-script.js");
            apiVersionDescriptionProvider.ApiVersionDescriptions.ToList().ForEach(description =>
            {
                cfg.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            });
        });
        app.UseStaticFiles();
        return app;
    }
}

