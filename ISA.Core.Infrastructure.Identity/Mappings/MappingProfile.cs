namespace ISA.Application.API.Mappings
{
    using AutoMapper;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Dtos.Company;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.Reservation;
    using ISA.Core.Domain.Entities.User;
    using System;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyUpdateDto>().ReverseMap();
            CreateMap<Company, CompanyProfileDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Equipment, EquipmentDto>().ReverseMap();
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<ReservationEquipment, EquipmentDto>().ForMember(d => d.Name, opt => opt.MapFrom(s => s.Equipment.Name)).ReverseMap();
            
        }
    }
}
