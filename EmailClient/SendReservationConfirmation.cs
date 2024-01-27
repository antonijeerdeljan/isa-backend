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
using System.Net.Mime;
using System.Reflection.Metadata;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF;
using System.Collections.Generic;
using System.Linq;
using ISA.Core.Domain.Entities.Reservation;
using ceTe.DynamicPDF;
using ISA.Core.Domain.UseCases.Reservation;
using ceTe.DynamicPDF.PageElements.BarCoding;

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
        string equipment = data?.Body;
        string name = data?.Name;
        string id = data?.Id;
        string time = data?.Time;

       Attachment attachment = GeneratePdf(equipment, name, id, time);

        try
        {
            string subject = Environment.GetEnvironmentVariable("key", EnvironmentVariableTarget.Process);
            string fromMail = "ftngrupa7@gmail.com";
            string fromPassword = "xowmkegadzjpwdrj";


            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = $"Potvrda porudzbine";
            message.To.Add(new MailAddress(email));
            message.Body = "U prilogu se nalazi dokument sa detaljima porudzbine.";
            message.Attachments.Add(attachment);

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);

            return new OkObjectResult(true);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }
    public static Attachment GeneratePdf(string equipment, string name, string id, string time)
    {

        var reservationEquipment = equipment.Split('?');

        ceTe.DynamicPDF.Document document = new ceTe.DynamicPDF.Document();
        Page page = new Page(PageSize.A4, PageOrientation.Portrait, 54.0f);
        DateTime appointment;
        DateTime.TryParse(time, out appointment);
        String content = new string("Postovani " + name + ",\nHvala Vam sto ste izvrsili narudzbu medicinske opreme kod nas. U nastavku mozete da vidite sve stavke vase narudzbe, a ispod se nalazi i qr kod sa kojim mozete preuzeti narudzbu. Preuzimanje se moze izvrsiti " + appointment.Date.ToLongDateString() + " u " + appointment.TimeOfDay.ToString());
        Label label = new Label("Potvrda porudzbine", 0, 0, 504, 100, Font.Helvetica, 30.0f, TextAlign.Center);
        Label header = new Label(content, 0, 60, 504, 100, Font.Helvetica, 12.0f, TextAlign.Left);
        page.Elements.Add(label);
        page.Elements.Add(header);
        Table2 table = new Table2(50, 150, 500, reservationEquipment.Count() * 30);
        table.Columns.Add(250); // Adjust the width based on your needs
        table.Columns.Add(150);  // Adjust the width based on your needs
        Row2 headerRow = table.Rows.Add();
        headerRow.Cells.Add("Naziv proizvoda");
        headerRow.Cells.Add("Kolicina");

        foreach (var r in reservationEquipment)
        {
            if (!string.IsNullOrEmpty(r))
            {
                var part = r.Split('/');
                Row2 dataRow = table.Rows.Add();
                dataRow.Cells.Add(part[0]);
                dataRow.Cells.Add(part[1]);
            }
        }

        page.Elements.Add(table);

        QrCode qrCode = new QrCode("www.facebook.com", 225, 170 + table.Height);
        page.Elements.Add(qrCode);
        // Add the page to the document
        document.Pages.Add(page);

        byte[] pdfBytes = ConvertPdfToByteArray(document);
        Attachment attachment = new Attachment(new MemoryStream(pdfBytes), "document.pdf");
        return attachment;
    }
    public static byte[] ConvertPdfToByteArray(ceTe.DynamicPDF.Document document)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            document.Draw(stream);
            return stream.ToArray();
        }
    }


}



