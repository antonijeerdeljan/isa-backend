namespace ISA.Core.Domain.Entities.Company;

public class CompanyEquipment
{
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; }
    public int Quantity { get; set; }
}
