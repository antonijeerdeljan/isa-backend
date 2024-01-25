using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Domain.Dtos.Company;

namespace ISA.Core.Domain.UseCases.Company;

public class AppointmentService 
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly CompanyService _companyService;
    private readonly UserService _userService;
    private readonly ICompanyAdminRepository _companyAdminRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepository, CompanyService companyService, UserService userService, IISAUnitOfWork isaUnitOfWork, IMapper mapper, ICompanyAdminRepository companyAdminRepository) 
    {
        _appointmentRepository = appointmentRepository;
        _companyService = companyService;
        _userService = userService;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
        _companyAdminRepository = companyAdminRepository;
    }

    public async Task AddAsync(AppointmentRequestModel appointment, Guid userId)
    {
        var compAdmin = await _companyAdminRepository.GetByIdAsync(userId);
        if (await _userService.IsUserIdInCompanyAdmins(appointment.AdminId, compAdmin.Company.Id) is false)
        {
            throw new ArgumentException();
        }
        if (await _companyService.IsAppointmentInWorkingHours(appointment.StartingDateTime, appointment.EndingDateTime, compAdmin.Company.Id) is false)
        {
            throw new ArgumentException();
        }
        {
            await _isaUnitOfWork.StartTransactionAsync();
            var company = await _companyService.GetCompanyAsync(compAdmin.Company.Id);
            var companyAdmin = await _companyAdminRepository.GetByIdAsync(appointment.AdminId);
            Appointment newAppointment = new Appointment(company, companyAdmin, appointment.StartingDateTime, appointment.EndingDateTime);
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

    public async Task<IEnumerable<AppointmentDto>> GetAllCompanyAppointments(int page, Guid adminId)
    {
        var admin = await _companyAdminRepository.GetByIdAsync(adminId);
        var appointments = _appointmentRepository.GetAllCompanyAppointments(page, admin.Company.Id);
        return appointments.Result.Select(appointment => _mapper.Map<AppointmentDto>(appointment));
    }


}
