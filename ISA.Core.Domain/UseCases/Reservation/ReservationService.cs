namespace ISA.Core.Domain.UseCases.Reservation;

using AutoMapper;
using ceTe.DynamicPDF;
using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Entities.Reservation;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.User;
using Nest;
using Newtonsoft.Json.Linq;

public class ReservationService
{
    private readonly EquipmentService _equipmentService;
    private readonly AppointmentService _appointmentService;
    private readonly UserService _userService;

    private readonly IHttpClientService _httpClientService;
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationEquipmentRepository _reservationEquipmentRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly IDocumentService _documentService;
    private readonly IMapper _mapper;

    public ReservationService(IHttpClientService httpClientService, EquipmentService equipmentService, IReservationRepository reservationRepository, UserService userService, AppointmentService appointmentService, IReservationEquipmentRepository reservationEquipmentRepository, IDocumentService documentService,IISAUnitOfWork isaUnitOfWork, IMapper mapper)
    {
        _httpClientService = httpClientService;
        _equipmentService = equipmentService;
        _reservationRepository = reservationRepository;
        _userService = userService;
        _appointmentService = appointmentService;
        _reservationEquipmentRepository = reservationEquipmentRepository;
        _documentService = documentService;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(Guid userId, Guid appointmentId, List<ReservationEquipmentRequest> requests)
    {
        var customer = await _userService.GetCustomerById(userId);
        var appointment = await _appointmentService.GetAppointmentById(appointmentId);
        var reserved = await _reservationRepository.GetByIdAsync(appointmentId);
        List<ReservationEquipment> reservationEquipment = new List<ReservationEquipment>();
        if (customer is null || appointment is null || reserved is not null)
        {
            throw new ArgumentNullException("Not good appointment");
        }

        await _isaUnitOfWork.StartTransactionAsync();

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
            Document pdf = _documentService.GeneratePdf(reservation.Equipments);
            await _httpClientService.SendReservationConfirmation(customer.User.Email, "Reservation confirmation", pdf);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
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

    private bool IsAppointmentWithin24Hours(Reservation reservation)
    {
        return (reservation.Appointment.StartingDateTime > DateTime.UtcNow.AddHours(24)) ? true : false;
    }
}
