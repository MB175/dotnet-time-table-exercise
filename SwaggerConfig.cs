using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

public static class SwaggerConfig
{
    public static void AddSwagger(this IServiceCollection services, string proxyBaseUrl)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Stundenplan API",
                Version = "v1",
                Description = "API zur Verwaltung von Stundenplänen nach Kalenderwoche"
            });

            if (!string.IsNullOrWhiteSpace(proxyBaseUrl))
            {
                options.AddServer(new OpenApiServer
                {
                    Url = proxyBaseUrl,
                    Description = "Reverse proxy path"
                });
            }
        });
    }

    public static void UseSwaggerWithUI(this WebApplication app, string swaggerJsonPath)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(swaggerJsonPath, "Stundenplan API v1");
            options.RoutePrefix = "swagger";
        });
    }
}
