using ISA.Core.Domain.Entities.Company;
using System.Drawing;

namespace ISA.Core.Domain.Entities.Delivery;

public class ContractEquipment
{
    public Guid ContractId { get; set; }
    public Contract Contract { get; set; }
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; }
    public int Quantity { get; set; }

    public ContractEquipment(Guid contractId, Guid equipmentId, int quantity)
    {
        ContractId = contractId;
        EquipmentId = equipmentId;
        Quantity = quantity;
    }

    public ContractEquipment(Contract contract,Equipment equipment, int quantity)
    {
        ContractId = contract.Id;
        Contract = contract;
        EquipmentId = equipment.Id;
        Equipment = equipment;
        Quantity = quantity;
    }
}
