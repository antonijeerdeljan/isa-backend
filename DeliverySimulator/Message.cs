using PolylineEncoder.Net.Models;
using System;

namespace DeliverySimulator;

public record Message(IGeoCoordinate coordinate,string companyId,string status);

