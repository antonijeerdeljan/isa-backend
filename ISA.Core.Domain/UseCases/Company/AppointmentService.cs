namespace ISA.Core.Domain.UseCases.Company
{
    using AutoMapper;
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Contracts;
    using ISA.Core.Domain.Dtos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;

    public class AppointmentService 
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IISAUnitOfWork _isaUnitOfWork;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository,ICompanyRepository companyRepository, IUserRepository userRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper) 
        {
            _appointmentRepository = appointmentRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _isaUnitOfWork = isaUnitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(AppointmentRequestModel appointment)
        {
            var company = await _companyRepository.GetByIdAsync(appointment.CompanyId);
            var user = await _userRepository.GetByIdAsync(appointment.AdminId);

            if (company != null && user!=null && company.Admins.Contains(user))
            {
                await _isaUnitOfWork.StartTransactionAsync();
                Appointment newAppointment = new Appointment(appointment.CompanyId, user.Id, user.Firstname, user.Lastname, appointment.DateTime, appointment.Duration);
                try
                {
                    await _appointmentRepository.AddAsync(newAppointment);
                    await _isaUnitOfWork.SaveAndCommitChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
            }


            

        }


    }
}
