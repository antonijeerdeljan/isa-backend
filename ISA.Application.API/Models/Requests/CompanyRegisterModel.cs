using ISA.Core.Domain.Entities.User;

namespace ISA.Application.API.Models.Requests;

public class CompanyRegisterModel
{
    public string Name { get; set; }
    public TimeOnly StartingWorkingHour { get; set; }
    public TimeOnly EndWorkingHour { get; set; }
    public string Description { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }

}


