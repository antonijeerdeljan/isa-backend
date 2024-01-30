using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Domain.Entities.User;
using Nest;
using ceTe.DynamicPDF;

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

    public async Task<Guid> TryCreateAppointment(Guid companyId, DateTime start, DateTime end)
    {
        
        if (await _companyService.IsAppointmentInWorkingHours(start,end, companyId) is false)
        {
            throw new ArgumentException();
        }
        try
        {
            //await _isaUnitOfWork.StartTransactionAsync();
            var company = await _companyService.GetCompanyAsync(companyId);
            var companyAdmins = await _companyAdminRepository.GetAllCompanyAdmins(companyId, 1);
            var appointments = await _appointmentRepository.GetAllCompanyAppointments(1, companyId);

            var newAppointment = CheckAdminAvailability(appointments, companyAdmins, company, start, end).Result;

            if (newAppointment is not null)
            {
                await _appointmentRepository.AddAsync(newAppointment);
                await _isaUnitOfWork.SaveAndCommitChangesAsync();
                return newAppointment.Id;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return Guid.Empty; 
        
    }

    public async Task RecycleAppointment(Guid appointmentId)
    {
        var appointment = await GetAppointmentById(appointmentId);
        Appointment newAppointment = new(appointment);
        await _appointmentRepository.AddAsync(newAppointment);
    }

    public async Task SetAsAvailable(Guid id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        appointment.SetAsAvailable();
        _appointmentRepository.Update(appointment);
    }

    public void UpdateAppointment(Appointment appointment)
    {
        _appointmentRepository.Update(appointment);
    }

    public async Task SetAsTaken(Guid id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        appointment.SetAsTaken();
        _appointmentRepository.Update(appointment);
    }

    public async Task<Appointment> GetAppointmentById(Guid id)
    {
        return await _appointmentRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllCompanyAppointments(int page, Guid adminId)
    {
        var admin = await _companyAdminRepository.GetByIdAsync(adminId);
        var appointments = _appointmentRepository.GetAllCompanyAppointments(page, admin.Company.Id);
        return appointments.Result.Select(appointment => _mapper.Map<AppointmentDto>(appointment));
    }

    public async Task<IEnumerable<Appointment>> GetAllCompanyAppointmentsForDate(int page, Guid companyId, DateOnly date)
    {
        return await _appointmentRepository.GetAllCompanyAppointmentsForDate(page, companyId, date);
    }

    public async Task<IEnumerable<AppointmentRequestModel>> GetPossibleAppointments(DateOnly date, Guid companyId)
    {
       
        try
        {
            if (IsWorkingDay(date) is false) throw new Exception();
            var appointments = await GetAllCompanyAppointmentsForDate(1, companyId, date) ?? throw new KeyNotFoundException();
            var admins = await _companyAdminRepository.GetAllCompanyAdmins(companyId, 1) ?? throw new KeyNotFoundException();
            var company = await _companyService.GetCompanyAsync(companyId) ?? throw new KeyNotFoundException();
            return await FindPossibleAppointments(date, appointments, admins, company);

        }
        catch (Exception e)
        {
            return (IEnumerable<AppointmentRequestModel>)e;
        }
       
    }

    public bool IsWithinOneHour(Appointment appointment)
    {
        return (DateTime.Now > appointment.StartingDateTime.AddHours(1)) ? false : true;
    }

    public bool IsWorkingDay(DateOnly date)
    {
        return !(date.Day is (int)DayOfWeek.Saturday || date.Day is (int)DayOfWeek.Sunday);
    }


    public async Task<IEnumerable<AppointmentRequestModel>> FindPossibleAppointments(DateOnly date,IEnumerable<Appointment> appointments, IEnumerable<CompanyAdmin> admins, Entities.Company.Company company)
    {
        appointments = appointments.OrderBy(a => a.StartingDateTime).ToList();
        List<AppointmentRequestModel> possibleTimeSlots = new List<AppointmentRequestModel>();
        foreach (var admin in admins)
        {
            var adminAppointments = appointments.Where(a => a.CompanyAdmin.UserId == admin.UserId).ToList();

            // Check if the administrator has any appointments
            if (adminAppointments.Count == 0)
            {
                // If no appointments, the entire day is a possible time slot
                DateTime start = new DateTime(date.Year, date.Month, date.Day, company.StartingWorkingHour.Hour, company.StartingWorkingHour.Minute, company.StartingWorkingHour.Second);
                DateTime end = new DateTime(date.Year, date.Month, date.Day, company.EndWorkingHour.Hour, company.EndWorkingHour.Minute, company.EndWorkingHour.Second);

                while (end - start >= TimeSpan.FromMinutes(30))
                {
                    // Gap is large enough, consider it as a possible time slot
                    Guid id = new Guid();
                    possibleTimeSlots.Add(new AppointmentRequestModel(start, start.AddMinutes(30), admin.UserId));
                    start = start.AddMinutes(30);
                }
            }
            else
            {
                // Check for gaps between appointments
                for (int i = 0; i < adminAppointments.Count - 1; i++)
                {
                    var gapStart = adminAppointments[i].EndingDateTime;
                    var gapEnd = adminAppointments[i + 1].StartingDateTime;

                    while (gapEnd - gapStart >= TimeSpan.FromMinutes(30))
                    {
                        // Gap is large enough, consider it as a possible time slot
                        Guid id = new Guid();
                        possibleTimeSlots.Add(new AppointmentRequestModel(gapStart, gapStart.AddMinutes(30), admin.UserId));
                        gapStart = gapStart.AddMinutes(30);
                    }
                }
            
                // Check if there's a gap at the beginning of the day
                if (adminAppointments[0].StartingDateTime.TimeOfDay - company.StartingWorkingHour.ToTimeSpan() >= TimeSpan.FromMinutes(30))
                {
                    var gapEnd = adminAppointments[0].StartingDateTime;
                    DateTime start = new DateTime(date.Year, date.Month, date.Day, company.StartingWorkingHour.Hour, company.StartingWorkingHour.Minute, company.StartingWorkingHour.Second);

                    while (gapEnd - start >= TimeSpan.FromMinutes(30))
                    {
                        // Gap is large enough, consider it as a possible time slot
                        Guid id = new Guid();
                        possibleTimeSlots.Add(new AppointmentRequestModel(start, start.AddMinutes(30), admin.UserId));
                        start = start.AddMinutes(30);
                    }
                }

                // Check if there's a gap at the end of the day
                if (company.EndWorkingHour.ToTimeSpan() - adminAppointments.Last().EndingDateTime.TimeOfDay >= TimeSpan.FromMinutes(30))
                {
                    DateTime end = new DateTime(date.Year, date.Month, date.Day, company.EndWorkingHour.Hour, company.EndWorkingHour.Minute, company.EndWorkingHour.Second);
                    DateTime start = new DateTime(date.Year, date.Month, date.Day, adminAppointments.Last().EndingDateTime.Hour, adminAppointments.Last().EndingDateTime.Minute, adminAppointments.Last().EndingDateTime.Second);

                    while (end - start >= TimeSpan.FromMinutes(30))
                    {
                        Guid id = new Guid();
                        possibleTimeSlots.Add(new AppointmentRequestModel(start, start.AddMinutes(30), admin.UserId));
                        start = start.AddMinutes(30);
                    }

                }
            }
        }
        return possibleTimeSlots.GroupBy(t => t.StartingDateTime)
                                 .Select(group => group.First())
                                 .OrderBy(t => t.StartingDateTime)
                                 .ToList(); ;
     }

    public async Task<Appointment> CheckAdminAvailability(IEnumerable<Appointment> appointments, IEnumerable<CompanyAdmin> admins, Entities.Company.Company company, DateTime start, DateTime end)
    {
        appointments = appointments.OrderBy(a => a.StartingDateTime).ToList();
        foreach (var admin in admins)
        {
            var adminAppointments = appointments.Where(a => a.CompanyAdmin.UserId == admin.UserId).ToList();
            // Check if the administrator has any appointments
            if (adminAppointments.Count == 0)
            {
                Appointment appointment = new Appointment(company, admin, start, end);
                return appointment;
            }
            else
            {
                // Check for gaps between appointments
                for (int i = 0; i < adminAppointments.Count - 1; i++)
                {
                    var gapStart = adminAppointments[i].EndingDateTime;
                    var gapEnd = adminAppointments[i + 1].StartingDateTime;

                    if (gapEnd >= end  && gapStart <= start)
                    {
                        Appointment appointment = new Appointment(company, admin, start, end);
                        return appointment;
                    }
                    
                }
                // Check if there's a gap at the beginning of the day
                if (adminAppointments[0].StartingDateTime >= end)
                {
                    Appointment appointment = new Appointment(company, admin, start, end);
                    return appointment;
                }

                // Check if there's a gap at the end of the day
                if (adminAppointments.Last().EndingDateTime <= start)
                {
                    Appointment appointment = new Appointment(company, admin, start, end);
                    return appointment;

                }
            }
        }
        return null;
        
    }

}
