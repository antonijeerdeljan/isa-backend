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
                string baseString = "https://localhost:7109/Users/VerifyEmail?email=" + email + "&token=" + token;

                MailMessage message = new MailMessage();
                message.Subject = "Thank You for Registering!";
                message.To.Add(new MailAddress(email));

                // Enhanced HTML content
                string htmlBody = @"
    <html>
        <head>
            <style>
                .email-container {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    padding: 20px;
                    text-align: center;
                }
                .email-content {
                    background-color: #ffffff;
                    padding: 40px;
                    margin: 20px;
                    border-radius: 8px;
                    box-shadow: 0px 0px 10px rgba(0,0,0,0.1);
                }
                .button {
                    background-color: #007bff; 
                    border: none;
                    color: white;
                    padding: 15px 32px;
                    text-align: center;
                    text-decoration: none;
                    display: inline-block;
                    font-size: 16px;
                    margin: 20px 0;
                    cursor: pointer;
                    border-radius: 4px;
                    transition: background-color 0.3s ease;
                }
                .button:hover {
                    background-color: #0056b3;
                }
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='email-content'>
                    <h1>Welcome!</h1>
                    <p>Thank you for registering. Please click the button below to verify your email address and get started:</p>
                    <a href='" + baseString + @"' class='button'>Verify Email</a>
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
}
