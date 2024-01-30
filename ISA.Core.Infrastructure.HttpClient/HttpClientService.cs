using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Infrastructure.HttpClients.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;
using ISA.Core.Domain.Entities.Reservation;


namespace ISA.Core.Infrastructure.HttpClients;

public class HttpClientService : IHttpClientService
{

    private readonly HttpClient _emailHttpClient;
    private readonly HttpClient _deliveryHttpClient;
    private readonly HttpClient _plainHttpClient;

    public HttpClientService(IHttpClientFactory httpClient)
    {
        _emailHttpClient = httpClient.CreateClient("AzureFunction1");
        _deliveryHttpClient = httpClient.CreateClient("AzureFunction2");
        _plainHttpClient = httpClient.CreateClient();
    }

    public async Task SendPickUpConfirmation(string email, string message, string name, string time, string companyName)
    {
        var payload = new EmailMessagePayload
        {
            Email = email,
            Message = message,
            Name = name,
            Time = time,
            CompanyName = companyName
        };

        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _emailHttpClient.PostAsync("Function3", content);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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
            var response = await _emailHttpClient.PostAsync("Function1", content);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Registration failed with status code: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Registration failed");
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
            var response = await _emailHttpClient.PostAsync("Function2", content);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task CreateDelivery(Point companyCoordinate, Guid companyId)
    {
        var payload = new
        {
            coordinates = new { companyCoordinate.X, companyCoordinate.Y },
            companyId
        };

        var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            _ = _deliveryHttpClient.PostAsync("Function1", content);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<Coordinate> GetCoordinatesFromAddress(string street, string city, string country, string number)
    {
        string address = $"{number} {street}, {city}, {country}";
        var apiKey = "AIzaSyBvavxN88RLP-X5aWZvK5ofSbfKvYG6M1c";
        string requestUri = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}";

        
        HttpResponseMessage response = await _plainHttpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(responseBody);

        if (json["results"] == null || !json["results"].Any())
        {
            throw new KeyNotFoundException("No results found for the specified address.");
        }

        var result = json["results"][0];
        var location = result["geometry"]["location"];
        double latitude = (double)location["lat"];
        double longitude = (double)location["lng"];

        return new Coordinate(longitude, latitude);
    }



}