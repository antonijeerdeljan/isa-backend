namespace ISA.Core.Domain.Dtos;

public class EquipmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }

    public EquipmentDto()
    {
    }
}
