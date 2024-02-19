namespace ISA.Core.Domain.Dtos.Company
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CompanyProfilesDto
    {
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public double AverageGrade { get; set; }
        public List<EquipmentDto>? Equipment { get; set; }
    }
}
