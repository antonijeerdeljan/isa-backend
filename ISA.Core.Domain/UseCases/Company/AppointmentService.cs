using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Contracts.Services;

namespace ISA.Core.Domain.UseCases.Company;

public class AppointmentService 
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICompanyAdminRepository _companyAdminRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepository,ICompanyRepository companyRepository, IUserRepository userRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper, ICompanyAdminRepository companyAdminRepository) 
    {
        _appointmentRepository = appointmentRepository;
        _companyRepository = companyRepository;
        _userRepository = userRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
        _companyAdminRepository = companyAdminRepository;
    }

    public async Task AddAsync(AppointmentRequestModel appointment)
    {
        var company = await _companyRepository.GetByIdAsync(appointment.CompanyId);
        var user = await _companyAdminRepository.GetByIdAsync(appointment.AdminId);

        if (company != null && user!=null && company.Admins.Contains(user))
        {
            await _isaUnitOfWork.StartTransactionAsync();
            Appointment newAppointment = new Appointment(appointment.CompanyId, user.Id, user.User.Firstname, user.User.Lastname, appointment.DateTime, appointment.Duration);
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
