namespace ISA.Application.API.Mappings
{
    using AutoMapper;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyUpdateDto>().ReverseMap();
            CreateMap<Company, CompanyProfileDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Equipment, EquipmentDto>().ReverseMap();
        }
    }
}
