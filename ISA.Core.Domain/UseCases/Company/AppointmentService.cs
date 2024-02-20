using AutoMapper;
using FluentResults;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Domain.UseCases.User;
using System.ComponentModel.Design;

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
        await _isaUnitOfWork.StartTransactionAsync();
        var companyAdmin = await _companyAdminRepository.GetByIdAsync(userId);
        if (await _userService.IsUserIdInCompanyAdmins(appointment.AdminId, companyAdmin.CompanyId) is false)  throw new ArgumentException();
        if (await _companyService.IsAppointmentInWorkingHours(appointment.StartingDateTime, appointment.EndingDateTime, companyAdmin.CompanyId) is false) throw new ArgumentException();   
        if (await CheckCompanyAvailability(companyAdmin.CompanyId, appointment) is false) throw new ArgumentException();
        var company = await _companyService.GetCompanyAsync(companyAdmin.CompanyId);
        //var companyAdmin = await _companyAdminRepository.GetByIdAsync(appointment.AdminId);
        Appointment newAppointment = new Appointment(company, companyAdmin, appointment.StartingDateTime, appointment.EndingDateTime);
        try
        {
            await _appointmentRepository.AddAsync(newAppointment);
            await _isaUnitOfWork.SaveAndCommitChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ArgumentException();
        }
        
    }

    public async Task<Appointment> TryCreateAppointment(Guid companyId, DateTime start, DateTime end)
    {

        if (await _companyService.IsAppointmentInWorkingHours(start, end, companyId) is false) throw new ArgumentException();
        try
        {
            var newAppointment = await CheckAndSetNewCompanyAppointment(companyId, start, end);
            if (newAppointment is null) return null;
            await _appointmentRepository.AddAsync(newAppointment);
            return newAppointment;

        }
        catch (Exception ex)
        {
            throw new ArgumentException();
        }


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
        var appointments = await _appointmentRepository.GetAllCompanyAppointments(admin.CompanyId);
        return appointments.Select(appointment => _mapper.Map<AppointmentDto>(appointment));
    }

    public async Task<IEnumerable<Appointment>> GetAllCompanyAppointmentsForDate(Guid companyId, DateOnly date)
    {
        return await _appointmentRepository.GetAllCompanyAppointmentsForDate(companyId, date);
    }

    public async Task<IEnumerable<AppointmentRequestModel>> GetPossibleAppointments(string dateString, Guid companyId)
    {
        if (DateOnly.TryParse(dateString, out DateOnly date) is false) throw new Exception();


        try
        {
            if (IsWorkingDay(date) is false) throw new Exception();
            return await FindPossibleAppointments(date, companyId);

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


    public async Task<IEnumerable<AppointmentRequestModel>> FindPossibleAppointments(DateOnly date, Guid companyId)
    { 
        var appointments = await GetAllCompanyAppointmentsForDate(companyId, date) ?? throw new KeyNotFoundException();
        var admins = await _companyAdminRepository.GetAllCompanyAdmins(companyId) ?? throw new KeyNotFoundException();
        var company = await _companyService.GetCompanyAsync(companyId) ?? throw new KeyNotFoundException();
        Random random = new Random();
        int number = random.Next(0, admins.Count()-1);

        appointments = appointments.OrderBy(a => a.StartingDateTime).ToList();
        List<AppointmentRequestModel> possibleTimeSlots = new List<AppointmentRequestModel>();
        
        // Check if the administrator has any appointments
        if (appointments.Count() == 0)
        {
            // If no appointments, the entire day is a possible time slot
            DateTime start = new DateTime(date.Year, date.Month, date.Day, company.StartingWorkingHour.Hour, company.StartingWorkingHour.Minute, company.StartingWorkingHour.Second);
            DateTime end = new DateTime(date.Year, date.Month, date.Day, company.EndWorkingHour.Hour, company.EndWorkingHour.Minute, company.EndWorkingHour.Second);

            while (end - start >= TimeSpan.FromMinutes(30))
            {
                // Gap is large enough, consider it as a possible time slot
                Guid id = new Guid();
                possibleTimeSlots.Add(new AppointmentRequestModel(start, start.AddMinutes(30), admins.ElementAt(number).UserId));
                start = start.AddMinutes(30);
            }
        }
        else
        {
            // Check for gaps between appointments
            for (int i = 0; i < appointments.Count() - 1; i++)
            {
                var gapStart = appointments.ElementAt(i).EndingDateTime;
                var gapEnd = appointments.ElementAt(i + 1).StartingDateTime;

                while (gapEnd - gapStart >= TimeSpan.FromMinutes(30))
                {
                    // Gap is large enough, consider it as a possible time slot
                    Guid id = new Guid();
                    possibleTimeSlots.Add(new AppointmentRequestModel(gapStart, gapStart.AddMinutes(30), admins.ElementAt(number).UserId));
                    gapStart = gapStart.AddMinutes(30);
                }
            }

            // Check if there's a gap at the beginning of the day
            if (appointments.ElementAt(0).StartingDateTime.TimeOfDay - company.StartingWorkingHour.ToTimeSpan() >= TimeSpan.FromMinutes(30))
            {
                var gapEnd = appointments.ElementAt(0).StartingDateTime;
                DateTime start = new DateTime(date.Year, date.Month, date.Day, company.StartingWorkingHour.Hour, company.StartingWorkingHour.Minute, company.StartingWorkingHour.Second);

                while (gapEnd - start >= TimeSpan.FromMinutes(30))
                {
                    // Gap is large enough, consider it as a possible time slot
                    Guid id = new Guid();
                    possibleTimeSlots.Add(new AppointmentRequestModel(start, start.AddMinutes(30), admins.ElementAt(number).UserId));
                    start = start.AddMinutes(30);
                }
            }

            // Check if there's a gap at the end of the day
            if (company.EndWorkingHour.ToTimeSpan() - appointments.Last().EndingDateTime.TimeOfDay >= TimeSpan.FromMinutes(30))
            {
                DateTime end = new DateTime(date.Year, date.Month, date.Day, company.EndWorkingHour.Hour, company.EndWorkingHour.Minute, company.EndWorkingHour.Second);
                DateTime start = new DateTime(date.Year, date.Month, date.Day, appointments.Last().EndingDateTime.Hour, appointments.Last().EndingDateTime.Minute, appointments.Last().EndingDateTime.Second);

                while (end - start >= TimeSpan.FromMinutes(30))
                {
                    Guid id = new Guid();
                    possibleTimeSlots.Add(new AppointmentRequestModel(start, start.AddMinutes(30), admins.ElementAt(number).UserId));
                    start = start.AddMinutes(30);
                }

            }
        
        }
        return possibleTimeSlots.GroupBy(t => t.StartingDateTime)
                                 .Select(group => group.First())
                                 .OrderBy(t => t.StartingDateTime)
                                 .ToList(); ;
    }

    public async Task<bool> CheckCompanyAvailability(Guid companyId, AppointmentRequestModel appointment)
    {
        var companyAppointments = await _appointmentRepository.GetAllCompanyAppointments(companyId);
        return companyAppointments.Where(a =>(a.StartingDateTime <= appointment.EndingDateTime.AddSeconds(-1) && a.EndingDateTime >= appointment.StartingDateTime.AddSeconds(1))).Count() == 0;
        
    }

    public async Task<Appointment> CheckAndSetNewCompanyAppointment(Guid companyId, DateTime start, DateTime end)
    {
        var companyAppointments = await _appointmentRepository.GetAllCompanyAppointments(companyId);
        if (companyAppointments.Where(a => a.EndingDateTime >= start && a.StartingDateTime <= end).Count() != 0) return null;
        var company = await _companyService.GetCompanyAsync(companyId);
        var admins = await _companyAdminRepository.GetAllCompanyAdmins(companyId);
        Random random = new Random();
        Int32 number = random.Next(0, admins.Count() - 1);
        Appointment appointment = new Appointment(company, admins.ElementAt(number), start, end);
        return appointment;
    }

}
