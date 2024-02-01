namespace ISA.Core.Domain.Dtos.Customer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CustomerProfileDto
    {
        public UserProfileDto User { get; set; }
        public int PenaltyPoints {  get; set; }
    }
}
