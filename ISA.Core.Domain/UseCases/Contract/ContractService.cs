using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos.Requests;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.Delivery;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.User;
using System.Reflection.Metadata.Ecma335;

namespace ISA.Core.Domain.UseCases.Contract;

public class ContractService
{
	private readonly IContractRepository _contractRepository;
	private readonly UserService _userService;
	private readonly EquipmentService _equipmentService;
    private readonly CompanyService _companyService;
	private readonly IISAUnitOfWork _unitOfWork;
    public ContractService(IContractRepository contractRepository, UserService userService, EquipmentService equipmentService, CompanyService companyService, IISAUnitOfWork unitOfWork)
	{
		_contractRepository = contractRepository;
		_userService = userService;
		_equipmentService = equipmentService;
		_unitOfWork = unitOfWork;
        _companyService = companyService;

    }

	public async Task AddContract(DateTime deliveryDate, List<GeneralEquipment> equipment, Guid userId)
	{

        await _unitOfWork.StartTransactionAsync();
		var companyAdmin = await _userService.GetCompanyAdmin(userId);

		if(await _contractRepository.CompanyAlreadyHaveContract(companyAdmin.CompanyId) is true)
		{
            await DeleteOldContract(companyAdmin);
        }
        
        var newEquipments = await ParseEquipments(equipment);

        await CreateContract(newEquipments, deliveryDate, companyAdmin);
        await _unitOfWork.SaveAndCommitChangesAsync();

    }
	private async Task CreateContract(List<Equipment> newEquipments, DateTime deliveryDate, CompanyAdmin companyAdmin)
	{
        Entities.Delivery.Contract contract = new();
        contract.Id = Guid.NewGuid();

        List<ContractEquipment> contractEquipment = new();

        foreach (var equipmentItem in newEquipments)
        {
            contractEquipment.Add(new ContractEquipment(contract.Id, equipmentItem.Id, (int)equipmentItem.Quantity));
        }

        contract.DeliveryDate = deliveryDate;
        contract.CreatedAt = DateTime.Now.ToUniversalTime();
        contract.Equipments = contractEquipment;
        contract.Company = await _companyService.GetCompanyAsync(companyAdmin.CompanyId);

        await _contractRepository.AddAsync(contract);
    }
    private async Task DeleteOldContract(CompanyAdmin companyAdmin)
    {
        var contract = await _contractRepository.GetContractByCompanyAsync(companyAdmin.CompanyId);
        foreach (var equipmentItem in contract.Equipments)
        {
            await _equipmentService.DeleteEquipment(equipmentItem.EquipmentId);
        }
        await _contractRepository.DeleteContract(contract.Id);
    }
    private async Task<List<Equipment>> ParseEquipments(List<GeneralEquipment> equipment)
    {
        List<Equipment> newEquipments = new();

        foreach (var equipmentItem in equipment)
        {
            Equipment newEquipment = await _equipmentService.GetById(equipmentItem.EquipmentId);
            newEquipment.Quantity = equipmentItem.Quantity;
            newEquipment.Id = Guid.NewGuid();
            newEquipments.Add(newEquipment);
            await _equipmentService.AddContractEquipmentAsync(newEquipment);
        }

        return newEquipments;
    }
    public async Task<Entities.Delivery.Contract> GetContractById(Guid id)
    {
        return await _contractRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
    }
    public async Task TransferEquipment(Guid companyId)
    {
        var contract = await _contractRepository.GetContractByCompanyAsync(companyId) ?? throw new KeyNotFoundException();
        foreach(var equipment in contract.Equipments)
        {
            var waitingEquipment = await _equipmentService.GetById(equipment.EquipmentId);
            waitingEquipment.CompanyId = companyId;
            await _equipmentService.UpdateAsync(waitingEquipment);
        }
    }

    public async Task<List<Entities.Delivery.Contract?>> GetTodaysContract()
    {
        return await _contractRepository.GetTodaysContract() ?? throw new KeyNotFoundException();
    }

    public async Task UpdateDeliveryTime(Guid contractId)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId) ?? throw new KeyNotFoundException();
        contract.DeliveredAt = DateTime.UtcNow;
        _contractRepository.Update(contract);
    }



}
