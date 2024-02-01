using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System;

namespace EmailClient;

public static class TempPasswordSender
{
    [FunctionName("Function4")]
    public static async Task<IActionResult> Run(
   [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
   ILogger log)
    {


        log.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string email = data?.Email;
        string name = data?.Name;
        string tempPassword = data?.Password;

        try
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true; // Set to true to enable HTML content
            message.Subject = "Change you password!";
            message.To.Add(new MailAddress(email));

            // HTML content for the email
            message.Body = $@"
        <html>
            <head>
                <style>
                    .email-container {{
                        font-family: Arial, sans-serif;
                        color: #333333;
                        background-color: #f4f4f4;
                        padding: 20px;
                        border-radius: 10px;
                        box-shadow: 0 4px 8px rgba(0,0,0,0.1);
                    }}
                    .email-header {{
                        background-color: #4CAF50;
                        color: white;
                        padding: 10px;
                        text-align: center;
                        border-radius: 10px 10px 0 0;
                    }}
                    .email-body {{
                        padding: 20px;
                        background-color: white;
                        text-align: center;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <div class='email-header'>Welcome!</div>
                    <div class='email-body'>
                        <p>Hello {name},</p>
                        <p>Your temporary password is: <strong>{tempPassword}</strong></p>
                        <p>Please change your password after your first login.</p>
                    </div>
                </div>
            </body>
        </html>";

            SmtpMailClient.Send(message);

            return new OkObjectResult(true);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }
}
