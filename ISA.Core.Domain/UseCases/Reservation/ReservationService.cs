﻿namespace ISA.Core.Domain.UseCases.Reservation;

using AutoMapper;
using ceTe.DynamicPDF;
using FluentResults;
using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.Reservation;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.User;

public class ReservationService
{
    private readonly EquipmentService _equipmentService;
    private readonly AppointmentService _appointmentService;
    private readonly UserService _userService;

    private readonly IHttpClientService _httpClientService;
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationEquipmentRepository _reservationEquipmentRepository;
    private readonly ICompanyAdminRepository _companyAdminRepository;
    private readonly ICompanyService _companyService;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly IMapper _mapper;

    public ReservationService(IHttpClientService httpClientService, EquipmentService equipmentService, IReservationRepository reservationRepository, UserService userService, AppointmentService appointmentService, IReservationEquipmentRepository reservationEquipmentRepository,  ICompanyAdminRepository companyAdminRepository, ICompanyService companyService, IISAUnitOfWork isaUnitOfWork, IMapper mapper)
    {
        _httpClientService = httpClientService;
        _equipmentService = equipmentService;
        _reservationRepository = reservationRepository;
        _userService = userService;
        _appointmentService = appointmentService;
        _reservationEquipmentRepository = reservationEquipmentRepository;
        _companyAdminRepository = companyAdminRepository;
        _companyService = companyService;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(Guid userId, Guid appointmentId, List<ReservationEquipmentRequest> requests)
    {
        await _isaUnitOfWork.StartTransactionAsync();

        var customer = await _userService.GetCustomerById(userId);
        var appointment = await _appointmentService.GetAppointmentById(appointmentId);
        var reserved = await _reservationRepository.GetByIdAsync(appointmentId);
        if (customer is null || appointment is null || reserved is not null || customer.PenaltyPoints >= 3) throw new ArgumentNullException();
        if (await CustomerIsAvailable(userId, appointment) is false) throw new ArgumentException("Already have appointment in that time");
        List<ReservationEquipment> reservationEquipment = new List<ReservationEquipment>();
        appointment.SetAsTaken();

        try
        {
            foreach(var r in requests)
            {
                if  (await _equipmentService.ExistEnough(r.EquipmentId, r.Quantity) is false)
                {
                    throw new ArgumentException("No enough equipment");
                }
                ReservationEquipment re = new ReservationEquipment(appointment.Id, r.EquipmentId, r.Quantity);
                reservationEquipment.Add(re);
            }
            
            Reservation reservation = new Reservation(appointment, customer, reservationEquipment);
            await _reservationRepository.AddAsync(reservation);
            foreach (var r in reservation.Equipments)
            {
                await _reservationEquipmentRepository.AddAsync(r);
                await _equipmentService.EquipmentSold(r.EquipmentId, r.Quantity);
            }
            await _isaUnitOfWork.SaveAndCommitChangesAsync();
            await _httpClientService.SendReservationConfirmation(customer.User.Email, "Reservation confirmation", reservation.Equipments, customer.User.Firstname, reservation.AppointmentId.ToString(), appointment.StartingDateTime.ToString());

        }
        catch (Exception ex)
        {
            throw new ArgumentException();
        }
    }

    public async Task AddExtraordinaryAsync(Guid userId, Guid companyId, DateTime start, DateTime end , List<ReservationEquipmentRequest> requests)
    {
        await _isaUnitOfWork.StartTransactionAsync();
        
        var customer = await _userService.GetCustomerById(userId);

        var appointment = await _appointmentService.TryCreateAppointment(companyId, start, end);
        List<ReservationEquipment> reservationEquipment = new List<ReservationEquipment>();
        if (customer is null || appointment is null || customer.PenaltyPoints >= 3) throw new ArgumentException();
        if (await CustomerIsAvailable(userId, appointment) is false) throw new ArgumentException("Already have appointment in that time");
        appointment.SetAsTaken();
        try
        {
            foreach (var r in requests)
            {
                if (await _equipmentService.ExistEnough(r.EquipmentId, r.Quantity) is false)
                {
                    throw new ArgumentException("No enough equipment");
                }
                ReservationEquipment re = new ReservationEquipment(appointment.Id, r.EquipmentId, r.Quantity);
                reservationEquipment.Add(re);
            }

            Reservation reservation = new Reservation(appointment, customer, reservationEquipment);
            await _reservationRepository.AddAsync(reservation);
            foreach (var r in reservation.Equipments)
            {
                await _reservationEquipmentRepository.AddAsync(r);
                await _equipmentService.EquipmentSold(r.EquipmentId, r.Quantity);
            }

            await _isaUnitOfWork.SaveAndCommitChangesAsync();
            await _httpClientService.SendReservationConfirmation(customer.User.Email, "Reservation confirmation", reservation.Equipments, customer.User.Firstname, reservation.AppointmentId.ToString(), appointment.StartingDateTime.ToString());

        }
        catch (Exception ex)
        {
            throw new ArgumentException();
        }
    }



    public async Task<List<Reservation>> OverdueReservations()
    {
        return await _reservationRepository.CheckForOverdueReservations();
    }

    public async Task SetReservationAsOverdue(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId) ?? throw new KeyNotFoundException();
        reservation.SetAsOverdue();
        _reservationRepository.Update(reservation);
    }

    public async Task CancelReservation(Guid userId, Guid reservationId)
    {
        await _isaUnitOfWork.StartTransactionAsync();

        var reservation = await _reservationRepository.GetByIdAsync(reservationId) ?? throw new KeyNotFoundException();
        reservation.SetAsCanceled();

        if(IsAppointmentWithin24Hours(reservation) is false)
        {
            await _userService.GivePenaltyPoints(userId, 1);
            await _appointmentService.RecycleAppointment(reservation.Appointment.Id);
        }
        else
        {
            await _userService.GivePenaltyPoints(userId, 2);
            if(_appointmentService.IsWithinOneHour(reservation.Appointment) is false)
            {
                await _appointmentService.RecycleAppointment(reservation.Appointment.Id);
            }
        }

        await _equipmentService.ReturnEqupment(reservation.Equipments);
        await _isaUnitOfWork.SaveAndCommitChangesAsync();
    }

    public async Task ReservationPickedUp(Guid userId, Guid reservationId)
    {
        await _isaUnitOfWork.StartTransactionAsync();
        var reservation = await _reservationRepository.GetByIdAsync(reservationId) ?? throw new KeyNotFoundException();
        if (reservation.Appointment.StartingDateTime.Date != DateTime.Now.Date) throw new KeyNotFoundException("Nije moguce ranije preuzeti rezervaciju");
        if (reservation.State != ReservationState.Pending) throw new KeyNotFoundException("Rezervacija je istekla ili je vec preuzeta");
        var appointment = await _appointmentService.GetAppointmentById(reservation.AppointmentId) ?? throw new KeyNotFoundException();
        var customer = await _userService.GetCustomerById(reservation.Customer.UserId) ?? throw new KeyNotFoundException();
        var company = await _companyService.GetCompanyAsync(appointment.Company.Id) ?? throw new KeyNotFoundException();
        if (appointment.CompanyAdmin.UserId == userId)
            reservation.SetAsFinished();
        else
        {
            throw new KeyNotFoundException("Nemate pravo predaje ove rezervacije.");
        }
        
        await _isaUnitOfWork.SaveAndCommitChangesAsync();
        await _httpClientService.SendPickUpConfirmation(customer.User.Email, "Pick up confirmation", customer.User.Firstname, appointment.StartingDateTime.ToString(), company.Name);

    }

    public async Task<IEnumerable<ReservationDto>> GetAllCompanyReservations(Guid adminId)
    {
        var admin = await _companyAdminRepository.GetByIdAsync(adminId);
        var reservations = await _reservationRepository.GetAllCompanyReservations(admin.CompanyId);
        var reservationDtos = reservations.Select(reservation => _mapper.Map<ReservationDto>(reservation));
        return reservationDtos;
    }


    public async Task<IEnumerable<ReservationDto>> GetAllCustomerReservations(Guid userId)
    {
        var reservations = await GetAllCustomerReservationsDomain(userId);
        var reservationDtos = reservations.Select(reservation => _mapper.Map<ReservationDto>(reservation));
        return reservationDtos;
    }


    public async Task<List<Reservation>> GetAllCustomerReservationsDomain(Guid userId)
    {
        return await _reservationRepository.GetAllCustomerReservations(userId);
    }

    public async Task<IEnumerable<ReservationDto>> GetHistoryOfCustomerReservations(Guid adminId, Guid customerId)
    {
        if (await CheckIfHavePolicy(adminId, customerId) is false) throw new KeyNotFoundException();
        var reservations = await _reservationRepository.GetHistoryOfCustomerReservations(customerId);
        var reservationDtos = reservations.Select(reservation => _mapper.Map<ReservationDto>(reservation));
        return reservationDtos;
    }

    public async Task<bool> UserHasAtleastOneReservationWithCompany(Guid userId, Guid companyId)
    {
        return await _reservationRepository.UserHasAtleastOneReservationWithCompany(userId, companyId);
    }

    public async Task<bool> UserHasAtleastOneReservationWithAdmin(Guid userId, Guid adminId)
    {
        return await _reservationRepository.UserHasAtleastOneReservationWithAdmin(userId, adminId);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllScheduledCustomerReservations(Guid userId)
    {
        var reservations = await _reservationRepository.GetAllScheduledCustomerReservations(userId);
        var reservationDtos = reservations.Select(reservation => _mapper.Map<ReservationDto>(reservation));
        return reservationDtos;
    }

    public async Task<ReservationDto> GetReservation(Guid reservationId, Guid userId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId) ?? throw new KeyNotFoundException();
        if ((reservation.Customer.UserId != userId && reservation.Appointment.CompanyAdmin.UserId != userId) is true) throw new KeyNotFoundException("Nemate pravo pristupa rezervaciji");
        return _mapper.Map<ReservationDto>(reservation);

    }

    public async Task<bool> CheckIfHavePolicy(Guid adminId, Guid customerId)
    {
        var reservations = await GetAllCustomerReservationsDomain(customerId);
        foreach (var reservation in reservations)
        {
            var companyAdmin = await _companyAdminRepository.GetByIdAsync(adminId);
            var appointment = await _appointmentService.GetAppointmentById(reservation.AppointmentId);
            if (appointment.Company.Id == companyAdmin.CompanyId) return true;
        }
        return false;
        
    }

    private bool IsAppointmentWithin24Hours(Reservation reservation)
    {
        return (reservation.Appointment.StartingDateTime > DateTime.UtcNow.AddHours(24)) ? true : false;
    }

    private async Task<bool> CustomerIsAvailable(Guid userId, Appointment appointment)
    {
        var customerAppointments = await _reservationRepository.GetAllCustomerReservations(userId);
        customerAppointments.RemoveAll(a => a.Appointment.EndingDateTime <= appointment.StartingDateTime || a.Appointment.StartingDateTime >= appointment.EndingDateTime);
        return customerAppointments.Count == 0;
       
    }
}
