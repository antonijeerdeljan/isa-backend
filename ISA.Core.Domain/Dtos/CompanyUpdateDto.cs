namespace ISA.Core.Domain.Dtos
{

    public class CompanyUpdateDto
    {
        public Guid Id {  get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public double AverageGrade { get; set; }

        public CompanyUpdateDto() { }
    }
}
