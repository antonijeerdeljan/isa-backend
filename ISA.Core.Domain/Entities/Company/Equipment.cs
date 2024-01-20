namespace ISA.Core.Domain.Entities.Company
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Equipment : Entity<Guid>
    {
        public string Name {  get; set; }
        public int Quantity { get; set; }
        public Guid CompanyId {  get; set; }

        public Equipment(string name, int quantity, Guid companyId)
        {
            Id = new Guid();
            Name = name;
            Quantity = quantity;
            CompanyId = companyId;
        }

        public Equipment()
        {
            Id = new Guid();
        }
    }
}
