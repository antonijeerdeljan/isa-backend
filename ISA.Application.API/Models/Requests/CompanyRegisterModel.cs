using ISA.Core.Domain.Entities.User;

namespace ISA.Application.API.Models.Requests;

public class CompanyRegisterModel
{
    public string Name { get; set; }
    public int StartingWorkingHour { get; set; }
    public int EndWorkingHour { get; set; }
    public string Description { get; set; }
    public string Country { get; set; }
    public string City { get; set; }    

}

