using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.UseCases.Contract;
using NetTopologySuite.Geometries;

namespace ISA.Core.Domain.UseCases.Delivery;

public class DeliveryService
{
    //treba da proveri da li je datum i vreme za dostavu kada ovaj treba da krene
    //ako jeste treba da napravi Point od kompanije i tako sto ce da gadja api
    //kada dobije point treba da gadja azure funkciju sa kordinatom i id-em
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
        var comapny = todaysContracts.FirstOrDefault();
        var address = comapny.Company.Address;

        DateTime now = DateTime.Now;

        DateTime noon = now.Date.AddHours(12); 

        var companyCord = await _httpClientService.GetCoordinatesFromAddress(address.Street, address.City,address.Country,address.Number.ToString());

        Point companyPoint = new(companyCord);

        await _httpClientService.CreateDelivery(companyPoint, comapny.Company.Id);

        if (now > noon)
        {
        }
        
    }
}
