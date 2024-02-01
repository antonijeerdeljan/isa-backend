using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using NetTopologySuite.Geometries;

namespace DeliverySimulator;

public static class HttpClientService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string apiKey = Environment.GetEnvironmentVariable("GoogleMapsAPIKey", EnvironmentVariableTarget.Process) ?? "AIzaSyBvavxN88RLP-X5aWZvK5ofSbfKvYG6M1c";

    public static async Task<string> GetPolyline(Point origin, Point destination)
    {
        string originStr = $"{origin.X},{origin.Y}";
        string destinationStr = $"{destination.X},{destination.Y}";
        string requestUri = $"https://maps.googleapis.com/maps/api/directions/json?origin={originStr}&destination={destinationStr}&key={apiKey}";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(responseBody);
        var polyline = json["routes"][0]["overview_polyline"]["points"].ToString();

        return polyline;
    }

}
