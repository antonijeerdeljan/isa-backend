namespace ISA.Core.Domain.Entities.User;

using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CompanyAdmin 
{
    [Key] //Do not change
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [Required]
    public Guid CompanyId { get; set; }

    public CompanyAdmin()
    {
    }

    public CompanyAdmin(Guid userId, Guid companyId)
    {
        UserId = userId;
        CompanyId = companyId;
    }
}
