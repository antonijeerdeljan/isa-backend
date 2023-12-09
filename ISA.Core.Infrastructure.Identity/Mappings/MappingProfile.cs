namespace ISA.Application.API.Mappings
{
    using AutoMapper;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Entities.Company;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyUpdateDto>().ReverseMap();
        }
    }
}
