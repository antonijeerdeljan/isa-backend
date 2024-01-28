using ISA.Core.Infrastructure.HttpClients;

namespace ISA.Application.API.Startup;

public static class HttpConfig
{
    public static IServiceCollection AddHttpClientConfig(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var baseUrl = builder.Configuration.GetSection("AzureEmailFunction:baseUrl").Value;
        var baseUrl1 = builder.Configuration.GetSection("AzureEmailFunction:baseUrl1").Value;


        services.AddHttpClient<HttpClientService>("AzureFunction1", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("x-functions-key", builder.Configuration.GetSection("AzureFunction1Settings:Key").Value);
        });

        // Configuration for Azure Function 2
        services.AddHttpClient<HttpClientService>("AzureFunction2", client =>
        {
            client.BaseAddress = new Uri(baseUrl1);
            client.DefaultRequestHeaders.Add("x-functions-key", builder.Configuration.GetSection("AzureFunction1Settings:Key").Value);
        });

        return services;
    }

}
