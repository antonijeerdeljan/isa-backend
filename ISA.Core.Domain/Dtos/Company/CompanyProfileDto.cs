namespace ISA.Core.Domain.Dtos.Company
{
    using ISA.Core.Domain.Entities.Company;
    using System.Collections.Generic;

    public class CompanyProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public double AverageGrade { get; set; }
        public List<AppointmentDto>? Appointments { get; set; }
        public List<EquipmentDto>? Equipment { get; set; }

    }
}
