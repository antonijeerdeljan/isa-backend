namespace EmailClient
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Mime;
 

    public static class SendReservationConfirmation
    {
        [FunctionName("Function2")]
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
                message1.Subject = $"Order confirmation!";
                message1.To.Add(new MailAddress(email));
                
                string baseString = "There is pdf file in attachment where you can see all order details.";
                message1.Body = baseString;



                var filePath = "C:\\Users\\I\\Desktop\\ISA projekat\\File.pdf";
                //byte[] bytes = File.ReadAllBytes(filePath);
                var emailAttachment = new Attachment(filePath, MediaTypeNames.Application.Pdf);

                message1.Attachments.Add(emailAttachment);

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

