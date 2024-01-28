using PolylineEncoder.Net.Models;

namespace ISA.Core.Domain.BackgroundTasks;

public record Message(GeoCoordinate coordinate, string companyId,string status);
