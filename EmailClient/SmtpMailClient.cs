using System.Net.Mail;
using System.Net;
using System;

namespace EmailClient;

public static class SmtpMailClient
{
    public static void Send(MailMessage message)
    {
        string fromMail = Environment.GetEnvironmentVariable("email", EnvironmentVariableTarget.Process);
        string fromPassword = Environment.GetEnvironmentVariable("key", EnvironmentVariableTarget.Process);

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromMail, fromPassword),
            EnableSsl = true
        };

        message.From = new MailAddress(fromMail);

        smtpClient.Send(message);
    }
}
