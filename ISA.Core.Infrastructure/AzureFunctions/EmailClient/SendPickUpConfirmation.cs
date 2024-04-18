namespace EmailClient;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

public static class SendPickUpConfirmation
{
    [FunctionName("Function3")]
    public static async Task<IActionResult> Run(
   [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
   ILogger log)
    {


        log.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string email = data?.Email;
        string name = data?.Name;
        string time = data?.Time;
        string companyName = data?.CompanyName;

        try
        {
 
            MailMessage message = new MailMessage();
            message.Subject = $"Potvrda preuzimanja";
            message.To.Add(new MailAddress(email));
            message.Body = "Postovani " + name + ",\nHvala Vam sto ste izvrsili preuzimanje narudzbe u terminu " + time + ".Nadamo se da ce Vas medicinska oprema dobro sluziti i ocekujemo ponovnu saradnju. \n\nSrdacan pozdrav,\n " + companyName + ".";
            SmtpMailClient.Send(message);

            return new OkObjectResult(true);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }
}
