namespace ISA.Application.API.Models.Requests
{
    public class UpdateEquipmentRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
