namespace ISA.Core.Domain.Entities.Company;

using System;

public class Equipment : Entity<Guid>
{
    public string Name {  get; set; }
    public int? Quantity { get; set; }
    public Company? Company {  get; set; }
    public bool IsDeleted { get; set; } = false;
    
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
    public Equipment(string name)
    {
        Name = name;
    }

    public Equipment(Guid id,string name, int quantity, Company company)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Company = company;
    }

    public void ReturnEquipment(int quantity)
    {
        Quantity = Quantity + quantity;
    }
        
    public Equipment(Guid id, string name, int quantity, Company company, bool deleted)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Company = company;
        IsDeleted = deleted;
    }
    public void Delete()
    {
        IsDeleted = true;
    }


}
