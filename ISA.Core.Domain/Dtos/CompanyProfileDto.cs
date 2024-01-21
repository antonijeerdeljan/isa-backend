namespace ISA.Core.Domain.Dtos
{
    using ISA.Core.Domain.Entities.Company;
    using System.Collections.Generic;

    public class CompanyProfileDto
    {
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public double AverageGrade { get; set; }
        public List<Appointment>? Appointments { get; set; }
        public List<EquipmentDto>? Equipment { get; set; }

    }
}
