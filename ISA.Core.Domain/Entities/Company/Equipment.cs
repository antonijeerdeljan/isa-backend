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
        public Company Company {  get; set; }

        public Equipment(string name, int quantity, Company company)
        {
            Id = new Guid();
            Name = name;
            Quantity = quantity;
            Company = company;
        }

        public Equipment()
        {
            Id = new Guid();
        }

        public Equipment(Guid id,string name, int quantity, Company company)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Company = company;
        }
    }
}
