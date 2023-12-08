using ISA.Core.Domain.Contracts;
using ISA.Core.Infrastructure.HttpClients.Entities;
using Newtonsoft.Json;
using System.Text;

namespace ISA.Core.Infrastructure.HttpClients;

public class HttpClientService : IHttpClientService
{
    public async Task SendEmail(string email, string message)
    {
        //http://localhost:7181/api/Function1
        //https://isaemailfunction.azurewebsites.net/api/Function1?code=vy_9frIZg9M1ZuDMOOFsnVOaAaVVafsbBJfKrhf8jI9fAzFuunztlA==
        var functionUrl = "http://localhost:7181/api/Function1";


        var payload = new EmailMessagePayload
        {
            Email = email,
            Message = message
        };

        // Serialize the payload to JSON
        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send the POST request
        using (var client = new HttpClient())
        {
            var response = await client.PostAsync(functionUrl, content);

            // Optionally, handle the response
            // For example, you can check the response status and act accordingly
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content);
            }
            else
            {
                // Handle error
            }
        }
    }

}