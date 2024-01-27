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
                //string fromMail = Environment.GetEnvironmentVariable("email", EnvironmentVariableTarget.Process);
                //string fromPassword = Environment.GetEnvironmentVariable("key", EnvironmentVariableTarget.Process);
                string fromMail = "ftngrupa7@gmail.com";
                string fromPassword = "xowmkegadzjpwdrj";

                var token = WebUtility.UrlEncode(body);
                string baseString = "https://localhost:7109/Users/VerifyEmail?email="+email+"&token="+token;

                //message.From = new MailAddress(fromMail);
                MailMessage message = new MailMessage();
                message.Subject = $"Thank for registering!";
                message.To.Add(new MailAddress(email));
                message.Body = baseString;

                /*var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                smtpClient.Send(message);*/

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
