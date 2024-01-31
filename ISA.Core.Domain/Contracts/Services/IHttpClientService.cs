namespace ISA.Core.Domain.Contracts.Services;

using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using NetTopologySuite.Geometries;
using ISA.Core.Domain.Entities.Reservation;
using ISA.Core.Domain.Entities.Complaint;

public interface IHttpClientService
{
    public Task SendRegistrationToken(string email, string message);

    Task CreateDelivery(Point companyCoordinate, Guid companyId);
  
    Task<Coordinate> GetCoordinatesFromAddress(string street,string city, string country, string number);

    //public Task SendReservationConfirmation(string email, string message, Document document);
    Task ComplaintSender(Complaint complaint, string answer);
    Task SendTempPassword(string email, string name, string password);
    public Task SendReservationConfirmation(string email, string message, List<ReservationEquipment> reservations, string name, string id, string time);

    public Task SendPickUpConfirmation(string email, string message, string Name, string time, string companyName);
}
