using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.UseCases.Contract;
using NetTopologySuite.Geometries;

namespace ISA.Core.Domain.UseCases.Delivery;

public class DeliveryService
{

    private readonly ContractService _contractService;
    private readonly IHttpClientService _httpClientService;

    public DeliveryService(ContractService contractService, IHttpClientService httpClientService)
    {
        _contractService = contractService;
        _httpClientService = httpClientService;
    }

    public async Task Delivery()
    {
        var todaysContracts = await _contractService.GetTodaysContract();

        foreach (var todaysContract in todaysContracts)
        {
            var contract = todaysContracts.FirstOrDefault() ?? throw new KeyNotFoundException();
            var address = contract.Company.Address;

            DateTime now = DateTime.Now;
            DateTime noon = now.Date.AddHours(12);

            if (now > noon)
            {
                var companyCord = await _httpClientService.GetCoordinatesFromAddress(
                    address.Street,
                    address.City,
                    address.Country,
                    address.Number.ToString()) ?? throw new KeyNotFoundException();

                Point companyPoint = new Point(companyCord); 
                await _httpClientService.CreateDelivery(companyPoint, contract.Company.Id);
                await _contractService.UpdateDeliveryTime(contract.Id);
            }
        }

    }
}
