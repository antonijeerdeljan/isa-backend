using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using PolylineEncoder.Net.Models;

namespace DeliverySimulator
{
    public class Function1
    {
        private readonly SendToMessageBus _sendToMessage;
        public Function1(SendToMessageBus sendToMessage)
        {
            _sendToMessage = sendToMessage;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            double? latitude = data?.coordinates?.x;
            double? longitude = data?.coordinates?.y;
            string? companyId = data?.companyId;

            Point pointDestination = null;
            if (latitude.HasValue && longitude.HasValue)
            {
                Coordinate destination = new Coordinate(latitude.Value, longitude.Value);
                pointDestination = new Point(destination);
            }

            Coordinate cord = new Coordinate(45.32648081639368, 20.447827235451125);
            Point point = new Point(cord);

            string responseMessage;
            List<IGeoCoordinate> cors = new();

            if (pointDestination != null)
            {
                cors = await PositionSimulator.GetNewComplexCoordinates(point, pointDestination);
                responseMessage = $"Received and processed coordinates: ({latitude}, {longitude}).";

                int totalCors = cors.Count;
                int currentCors = 0;

                foreach (var c in cors)
                {
                    currentCors++;
                    Message message = new(c, companyId, "driving");
                    _sendToMessage.Send(message);

                    // If it's the last item in the loop, send "done" status
                    if (currentCors == totalCors)
                    {
                        Message doneMessage = new(c, companyId, "done");
                        _sendToMessage.Send(doneMessage);
                    }
                    else
                    {
                        await Task.Delay(5000); // Delay for 5 seconds between messages
                    }
                }
            }
            else
            {
                responseMessage = "No valid destination coordinates provided.";
            }

            return new OkObjectResult(responseMessage);
        }

    }
}
