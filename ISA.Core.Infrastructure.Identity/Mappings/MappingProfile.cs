namespace ISA.Application.API.Mappings
{
    using AutoMapper;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Dtos.Company;
    using ISA.Core.Domain.Dtos.Complaint;
    using ISA.Core.Domain.Dtos.Customer;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.Complaint;
    using ISA.Core.Domain.Entities.Reservation;
    using ISA.Core.Domain.Entities.User;
    using System;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyUpdateDto>().ReverseMap();
            CreateMap<Company, CompanyProfileDto>().ReverseMap();
            CreateMap<Company, CompanyProfilesDto>().ReverseMap();
            CreateMap<Company, CompanyBasicInfoDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Equipment, EquipmentDto>().ReverseMap();
            CreateMap<Complaint, ComplaintDto>().ReverseMap();
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<Customer, CustomerProfileDto>().ReverseMap();
            CreateMap<User, UserProfileDto>().ReverseMap();
            CreateMap<Grade, GradeDto>().ReverseMap();
            CreateMap<ReservationEquipment, EquipmentDto>().ForMember(d => d.Name, opt => opt.MapFrom(s => s.Equipment.Name)).ReverseMap();
            
        }
    }
}
