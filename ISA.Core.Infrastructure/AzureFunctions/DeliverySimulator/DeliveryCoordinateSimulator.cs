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
using DeliverySimulator.MessageAndEventBus;
using System.Linq;

namespace DeliverySimulator
{
    public class DeliveryCoordinateSimulator
    {
        private readonly MessageBus _sendToMessage;
        public DeliveryCoordinateSimulator(MessageBus sendToMessage)
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

            double? latitude = data?.coordinates?.y;
            double? longitude = data?.coordinates?.x;
            string? companyId = data?.companyId;

            Point pointDestination = null;
            if (latitude.HasValue && longitude.HasValue)
            {
                Coordinate destination = new Coordinate((double)latitude, (double)longitude);
                pointDestination = new Point(destination);
            }

            Coordinate cord = new Coordinate(45.24402765011596, 19.841881760213614); //init position Novi sad Promenada
            Point point = new Point(cord);
            try
            {
                string responseMessage;
                List<IGeoCoordinate> cors = new();

                if (pointDestination != null)
                {
                    cors = await CoordinateSimulator.GetCoordinates(point, pointDestination);
                    responseMessage = $"Received and processed coordinates: ({latitude}, {longitude}).";

                    foreach (var c in cors)
                    {
                        Message message = new(c, companyId, "driving");
                        _sendToMessage.Send(message);
                        await Task.Delay(5000);
                    }

                    Message doneMessage = new(cors.LastOrDefault(), companyId, "done");
                    _sendToMessage.Send(doneMessage);

                }
                else
                {
                    responseMessage = "No valid destination coordinates provided.";
                }
            }catch(Exception ex)
            {
                throw new ArgumentException();
            }

            return new OkObjectResult("ok");
        }

    }
}
