using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace EmailClient
{
    public static class RegistrationTokenSender
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string email = data?.Email;
            string body = data?.Message;

            try
            {
                var token = WebUtility.UrlEncode(body);
                string baseString = "https://localhost:7109/Users/VerifyEmail?email="+email+"&token="+token;

                MailMessage message = new MailMessage();
                message.Subject = $"Thank for registering!";
                message.To.Add(new MailAddress(email));
                message.Body = baseString;

                SmtpMailClient.Send(message);

                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

        }
    }
}
