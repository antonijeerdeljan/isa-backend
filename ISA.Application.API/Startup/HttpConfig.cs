using ISA.Core.Infrastructure.HttpClients;

namespace ISA.Application.API.Startup;

public static class HttpConfig
{
    public static IServiceCollection AddHttpClientConfig(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var baseUrl = builder.Configuration.GetSection("AzureEmailFunction:baseUrl").Value;

        services.AddHttpClient<HttpClientService>(nameof(HttpClientService), x => {
            x.BaseAddress = new Uri(baseUrl);
            x.DefaultRequestHeaders.Add("x-functions-key", builder.Configuration.GetSection("AzureEmailFunctionSettings:Key").Value);
        });

        return services;
    }

}
