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
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;
using System.Text.Encodings.Web;

namespace EmailClient
{
    public static class EmailSender
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {


            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string email = data?.Email;
            string message = data?.Message;

            try
            {
                //string fromMail = Environment.GetEnvironmentVariable("email", EnvironmentVariableTarget.Process);
                //string fromPassword = Environment.GetEnvironmentVariable("key", EnvironmentVariableTarget.Process);
                string fromMail = "ftngrupa7@gmail.com";
                string fromPassword = "xowmkegadzjpwdrj";

                MailMessage message1 = new MailMessage();
                message1.From = new MailAddress(fromMail);
                message1.Subject = $"Thank for registering!";
                message1.To.Add(new MailAddress(email));
                var token = WebUtility.UrlEncode(message);
                string baseString = "https://localhost:7109/Users/VerifyEmail?email="+email+"&token="+token;
                message1.Body = baseString;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                smtpClient.Send(message1);

                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

        }
    }
}
