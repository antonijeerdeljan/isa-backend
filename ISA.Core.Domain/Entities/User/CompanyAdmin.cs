namespace ISA.Core.Domain.Entities.User;

using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CompanyAdmin 
{
    [Key]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    public Company.Company Company { get; set; }

    public CompanyAdmin()
    {
    }

    public CompanyAdmin(Guid userId, Company.Company company)
    {
        UserId = userId;
        Company = company;
    }
}
