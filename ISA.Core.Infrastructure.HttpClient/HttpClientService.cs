using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Infrastructure.HttpClients.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ceTe.DynamicPDF;
using System.Text;
using Nest;


namespace ISA.Core.Infrastructure.HttpClients;

public class HttpClientService : IHttpClientService
{

    private readonly HttpClient _httpClient;
    public HttpClientService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient(nameof(HttpClientService));
    }

    public async Task SendRegistrationToken(string email, string message)
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }       
    }

    public async Task SendReservationConfirmation(string email, string message, Document document)
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
            var response = await _httpClient.PostAsync("Function2", content);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    
}