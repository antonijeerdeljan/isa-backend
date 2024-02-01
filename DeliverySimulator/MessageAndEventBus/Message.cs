using PolylineEncoder.Net.Models;
using System;

namespace DeliverySimulator.MessageAndEventBus;

public record Message(IGeoCoordinate coordinate, string companyId, string status);

