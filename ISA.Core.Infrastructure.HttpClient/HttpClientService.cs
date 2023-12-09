using ISA.Core.Domain.Contracts;
using ISA.Core.Infrastructure.HttpClients.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;


namespace ISA.Core.Infrastructure.HttpClients;

public class HttpClientService : IHttpClientService
{

    private readonly HttpClient _httpClient;
    public HttpClientService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient(nameof(HttpClientService));
    }

    public async Task SendEmail(string email, string message)
    {
        var payload = new EmailMessagePayload
        {
            Email = email,
            Message = message
        };

        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        { 
            var response = await _httpClient.PostAsync("Function1", content);
        }catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

            
    }

}