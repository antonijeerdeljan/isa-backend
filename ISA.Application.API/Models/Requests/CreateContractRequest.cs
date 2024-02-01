using ISA.Core.Domain.Dtos.Requests;

namespace ISA.Application.API.Models.Requests;

public class CreateContractRequest
{
    public DateTime DeliveryDate { get; set; }
    public List<GeneralEquipment> Equipments { get; set; }

}
