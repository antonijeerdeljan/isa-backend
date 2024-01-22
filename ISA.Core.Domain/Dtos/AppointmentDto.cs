namespace ISA.Core.Domain.Dtos
{
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AppointmentDto
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public AppointmentDto() { }
    }
}
