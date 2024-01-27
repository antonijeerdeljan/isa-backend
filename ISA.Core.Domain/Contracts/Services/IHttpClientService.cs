namespace ISA.Core.Domain.Contracts.Services;

using ceTe.DynamicPDF;
using ISA.Core.Domain.Entities.Reservation;

public interface IHttpClientService
{
    public Task SendEmail(string email, string message);

    public Task SendReservationConfirmation(string email, string message, List<ReservationEquipment> reservations, string Name, string Id, string time);
}
