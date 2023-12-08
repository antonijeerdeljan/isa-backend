using ISA.Core.Domain.Contracts;
using ISA.Core.Infrastructure.HttpClients.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;


namespace ISA.Core.Infrastructure.HttpClients;

public class HttpClientService : IHttpClientService
{
    private readonly IConfiguration _configuration;
    public HttpClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendEmail(string email, string message)
    {
        //http://localhost:7181/api/Function1
        var key = _configuration.GetSection("AzureEmailFunctionSettings").GetSection("Key").Value;
        var functionUrl = "https://isaemailfunction.azurewebsites.net/api/Function1?code=" + key;
        //var functionUrl = "http://localhost:7181/api/Function1";

        var payload = new EmailMessagePayload
        {
            Email = email,
            Message = message
        };

        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using (var client = new HttpClient())
        {
            var response = await client.PostAsync(functionUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content);
            }
            
        }
    }

}