using System.Reflection;
using FluentValidation;
using ConnectApiApp.Common;
using ConnectApiApp.Common.Behaviors;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConnectApiApp.Common.Swagger;
using ConnectApiApp.Common.Caching;
using LsConnectClient;

namespace ConnectApiApp;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        
        services.AddSwaggerFeatures();

        services.AddHttpContextAccessor();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddConnectionBehaviors();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddTransient<IWcfClient, WcfClient>(x=> new WcfClient(builderConfiguration["ConnectService:EndpointAddress"]));

        services.AddApiVersioning(
            options => { options.ReportApiVersions = true; });
        services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        //this will make the route appear in lowercase in Swagger
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddMemoryCache();
        services.AddHostedService<DatabaseNamesCacheService>();
        services.AddSingleton<DatabaseNamesCacheService>();

        return services;
    }

    public static IServiceCollection AddConnectionBehaviors(this IServiceCollection services)
    {
        services.AddScoped<TenantContext>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CustomerDatabaseBehavior<,>));
        
        return services;
    }
}