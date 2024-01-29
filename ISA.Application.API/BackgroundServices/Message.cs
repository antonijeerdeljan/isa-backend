namespace ISA.Application.API.BackgroundServices;

public record Message(GeoCoordinate coordinate, string companyId, string status);

