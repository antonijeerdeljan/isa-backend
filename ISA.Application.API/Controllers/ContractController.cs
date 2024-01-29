using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Delivery;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ISA.Application.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContractController : ControllerBase
{
    private readonly ContractService _contractService;
    private readonly IHttpContextAccessor _contextAccessor;
    public ContractController(ContractService contractService, IHttpContextAccessor contextAccessor)
    {
        _contractService = contractService;
        _contextAccessor = contextAccessor;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task CreateContract([FromBody] CreateContractRequest contractRequest)
    {
        Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        await _contractService.AddContract(contractRequest.DeliveryDate, contractRequest.Equipments, userId);
    }


    




}
