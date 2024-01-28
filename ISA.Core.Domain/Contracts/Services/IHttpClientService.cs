namespace ISA.Core.Domain.Contracts.Services;

using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using NetTopologySuite.Geometries;

public interface IHttpClientService
{
    public Task SendRegistrationToken(string email, string message);

    Task CreateDelivery(Point companyCoordinate, Guid companyId);
    Task<Coordinate> GetCoordinatesFromAddress(string street,string city, string country, string number);
    public Task SendReservationConfirmation(string email, string message, Document document);
}
