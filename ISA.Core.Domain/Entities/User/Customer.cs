namespace ISA.Core.Domain.Entities.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Customer 
{
    [Key] //Do not change
    public Guid UserId {  get; set; }
    public string Profession { get; set; }
    public string CompanyInfo { get; set; }
    public Entities.LoyaltyProgram.LoyaltyProgram? LoyaltyProgram { get; set; }
    public int? PenaltyPoints { get; set; } = 0;
    public int? Points { get; set; } = 0;
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public Customer()
    {

    }

    public Customer(Guid userId,string profession, string companyInfo, User user)
    {
        UserId = userId;
        Profession = profession;
        CompanyInfo = companyInfo;
        User = user;
    }
}
