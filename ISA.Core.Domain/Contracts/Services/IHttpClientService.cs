namespace ISA.Core.Domain.Contracts.Services;

using ceTe.DynamicPDF;

public interface IHttpClientService
{
    public Task SendEmail(string email, string message);

    public Task SendReservationConfirmation(string email, string message, Document document);
}
