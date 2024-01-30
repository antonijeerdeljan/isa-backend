namespace ISA.Core.Domain.Entities.Company;

using System;

public class Equipment : Entity<Guid>
{
    public string Name {  get; set; }
    public int? Quantity { get; set; }
    public Guid? CompanyId {  get; set; }
    public bool IsDeleted { get; set; } = false;
    
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
    public Equipment(string name)
    {
        Name = name;
    }

    public Equipment(Guid id,string name, int quantity, Guid companyId)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        CompanyId = companyId;
    }

    public void ReturnEquipment(int quantity)
    {
        Quantity = Quantity + quantity;
    }
        

    public void Delete()
    {
        IsDeleted = true;
    }


}
