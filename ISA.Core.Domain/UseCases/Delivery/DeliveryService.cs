﻿using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.UseCases.Contract;
using NetTopologySuite.Geometries;

namespace ISA.Core.Domain.UseCases.Delivery;

public class DeliveryService
{

    private readonly ContractService _contractService;
    private readonly IHttpClientService _httpClientService;
    private readonly IISAUnitOfWork _unitOfWork;

    public DeliveryService(ContractService contractService, IHttpClientService httpClientService, IISAUnitOfWork unitOfWork)
    {
        _contractService = contractService;
        _httpClientService = httpClientService;
        _unitOfWork = unitOfWork;
    }

    public async Task Delivery()
    {


        var todaysContracts = await _contractService.GetTodaysContract();

        if(todaysContracts != null)
        {
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
                        address.Number.ToString());


                    if(companyCord is null) {
                        companyCord = new Coordinate(45.259832216479296, 19.807517841677335); //just in case of failure
                    }

                    Point companyPoint = new Point(companyCord);

                    await _unitOfWork.StartTransactionAsync();

                    await _httpClientService.CreateDelivery(companyPoint, contract.Company.Id);
                    await _contractService.UpdateDeliveryTime(contract.Id);

                    await _unitOfWork.SaveAndCommitChangesAsync();
                }
            }
        }
    }
}
