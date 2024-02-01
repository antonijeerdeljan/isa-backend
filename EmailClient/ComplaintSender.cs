using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using ceTe.DynamicPDF.PageElements.Charting;
using ISA.Core.Domain.Entities.Complaint;
using Xamarin.Essentials;

namespace EmailClient;

public static class ComplaintSender
{
    [FunctionName("Function5")]
    public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
    {


        // Read the request body
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        // Extract data from the request
        string email = data?.Email;
        string title = data?.Title;
        string description = data?.Description;
        string answer = data?.Answer;
        string adminName = data?.AdminName;

        try
        {
            MailMessage message = new MailMessage();
            message.Subject = "Answer to Complaint!";
            message.To.Add(new MailAddress(email));

            string htmlBody = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4; color: #333; }}
                    .container {{ max-width: 600px; margin: auto; background: white; border: 1px solid #ddd; padding: 20px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                    .header {{ background-color: #007bff; color: white; padding: 15px; text-align: center; }}
                    .header h2 {{ margin: 0; }}
                    .content {{ padding: 20px; line-height: 1.6; }}
                    .content strong {{ color: #007bff; }}
                    .footer {{ background-color: #f2f2f2; padding: 10px; text-align: center; font-size: 0.9em; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Response to Your Complaint</h2>
                    </div>
                    <div class='content'>
                        <p><strong>Complaint Title:</strong> {title}</p>
                        <p><strong>Description:</strong><br>{description}</p>
                        <p><strong>Response:</strong><br>{answer}</p>
                    </div>
                    <div class='footer'>
                        <p>Best regards,<br>{adminName}</p>
                    </div>
                </div>
            </body>
            </html>";


            message.Body = htmlBody;
            message.IsBodyHtml = true;

            SmtpMailClient.Send(message);

            return new OkObjectResult(true);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }



    }
}
