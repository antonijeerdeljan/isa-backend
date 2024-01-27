using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Infrastructure.HttpClients.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ceTe.DynamicPDF;
using System.Text;
using ISA.Core.Domain.Entities.Reservation;
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

    public async Task SendReservationConfirmation(string email, string message, List<ReservationEquipment> reservations, string name, string id, string time)
    {
        string equipment = String.Empty;
        foreach(var r in reservations)
        {
            equipment += r.Equipment.Name + "/" + r.Quantity.ToString() + "?";
        }


        var payload = new EmailMessagePayload
        {
            Email = email,
            Message = message,
            Body = equipment,
            Name = name,
            Id = id.ToString(),
            Time = time

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