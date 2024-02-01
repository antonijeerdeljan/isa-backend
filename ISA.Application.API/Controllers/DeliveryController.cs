using ISA.Core.Domain.UseCases.Delivery;
using Microsoft.AspNetCore.Mvc;

namespace ISA.Application.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveryController : ControllerBase
{
	private readonly DeliveryService _deliveryService;
	public DeliveryController(DeliveryService deliveryService)
	{
		_deliveryService = deliveryService;
	}

	[HttpGet("start-delivery")]
	public async Task StartDelivery()
	{
		await _deliveryService.Delivery();
	}

}
