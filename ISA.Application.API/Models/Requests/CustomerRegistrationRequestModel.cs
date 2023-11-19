namespace ISA.Application.API.Models.Requests;

public class CustomerRegistrationRequestModel : RegistrationRequestModel
{
    public string Profession { get; set; }
    public string CompanyInfo { get; set; }

    public CustomerRegistrationRequestModel(string profession, string companyInfo) : base()
    {
        Profession = profession;
        CompanyInfo = companyInfo;
    }
}
