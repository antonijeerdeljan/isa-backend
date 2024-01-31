using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos.Admin;
using ISA.Core.Domain.Dtos.Company;
using ISA.Core.Domain.Dtos.Complaint;
using ISA.Core.Domain.Entities.Complaint;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.Reservation;
using ISA.Core.Domain.UseCases.User;

namespace ISA.Core.Domain.UseCases.Complaint;

public class ComplaintService
{
	private readonly ReservationService _reservationService;
    private readonly CompanyService _companyService;
    private readonly IComplaintRepository _complaintRepository;
	private readonly IMapper _mapper;
    private readonly UserService _userService;
    private readonly IISAUnitOfWork _unitOfWork;
    private readonly IHttpClientService _httpClientService;
    public ComplaintService(ReservationService reservationService, IISAUnitOfWork unitOfWork, IHttpClientService httpClientService, IComplaintRepository complaintRepository, IMapper mapper, UserService userService, CompanyService companyService)
	{
		_reservationService = reservationService;
		_mapper = mapper;
        _userService = userService;
        _complaintRepository = complaintRepository;
        _companyService = companyService;
        _httpClientService = httpClientService;
        _unitOfWork = unitOfWork;
	}

	private async Task<List<Entities.Reservation.Reservation>> GetAllCustomerReservations(Guid userId)
	{
		return await _reservationService.GetAllCustomerReservationsDomain(userId);
    }


    public async Task<List<CompanyBasicInfoDto>> GetPossibleComplaintCompanies(Guid userId)
    {
        List<CompanyBasicInfoDto> companies = new List<CompanyBasicInfoDto>();

        var reservations = await GetAllCustomerReservations(userId);

        foreach (var reservation in reservations)
        {
            var companyDto = _mapper.Map<CompanyBasicInfoDto>(reservation.Appointment.Company);

            if (companyDto != null)
            {
                companies.Add(companyDto);
            }
        }
        return companies.GroupBy(c => c.Id)
                    .Select(group => group.First())
                    .ToList();
    }

    public async Task<List<CompanyAdminDto>> GetPossibleComplaintAdmins(Guid userId)
    {
        List<CompanyAdminDto> admins = new();

        var reservations = await GetAllCustomerReservations(userId);

        foreach (var reservation in reservations)
        {

            admins.Add(new CompanyAdminDto(reservation.Appointment.CompanyAdmin.UserId,
                                           reservation.Appointment.CompanyAdmin.User.Firstname,
                                           reservation.Appointment.CompanyAdmin.User.Lastname));
        }

        return admins.GroupBy(a => a.adminId)
                .Select(group => group.First())
                .ToList();
    }

    public async Task CreateCompanyComplaint(Guid userId, Guid companyId, string title, string description)
    {
        //api zastita za customerea
        var customer = await _userService.GetCustomerById(userId);
        if(await _reservationService.UserHasAtleastOneReservationWithCompany(userId,companyId) != true)
        {
            throw new ArgumentException();
        }
        Entities.Complaint.Complaint complaint = new Entities.Complaint.Complaint(ComplaintType.Compnay,title,customer,description,companyId);
        await _complaintRepository.AddAsync(complaint);
        _complaintRepository.UpdateAndSaveChanges(complaint);
    }

    public async Task CreateAdminComplaint(Guid userId, Guid adminId, string title, string description)
    {
        //api zastita za customerea
        var customer = await _userService.GetCustomerById(userId);
        if (await _reservationService.UserHasAtleastOneReservationWithAdmin(userId, adminId) != true)
        {
            throw new ArgumentException();
        }
        Entities.Complaint.Complaint complaint = new Entities.Complaint.Complaint(ComplaintType.Admin, title, customer, description, adminId);
        await _complaintRepository.AddAsync(complaint);
        _complaintRepository.UpdateAndSaveChanges(complaint);
    }




    public async Task<List<ComplaintDto>> GetAllCompanyComplaints(Guid adminId, int page)
    {
        var company = await _companyService.GetCompanyByAdminIdAsync(adminId);
        var complaints =  await _complaintRepository.GetAllCompanyComplaints(company.Id,page);
        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;
    }

    public async Task<List<ComplaintDto>> GetUnansweredCompanyComplaints(Guid adminId, int page)
    {
        var company = await _companyService.GetCompanyByAdminIdAsync(adminId);
        var complaints = await _complaintRepository.GetUnansweredCompanyComplaints(company.Id, page);
        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;

    }

    public async Task<List<ComplaintDto>> GetAnsweredCompanyComplaints(Guid adminId, int page)
    {

        var company = await _companyService.GetCompanyByAdminIdAsync(adminId);
        var complaints = _complaintRepository.GetAnsweredCompanyComplaints(company.Id, page);
        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;
    }

    public async Task<List<ComplaintDto>> GetAllAdminComplaints(Guid adminId, int page)
    {
        var complaints =  await _complaintRepository.GetAllAdminComplaints(adminId, page);
        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;
    }

    public async Task<List<ComplaintDto>> GetUnansweredAdminComplaints(Guid adminId, int page)
    {
        var complaints = await _complaintRepository.GetUnansweredAdminComplaints(adminId, page);

        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;
    }

    public async Task<List<ComplaintDto>> GetAnsweredAdminComplaints(Guid adminId, int page)
    {
        var complaints = await _complaintRepository.GetAnsweredAdminComplaints(adminId, page);

        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;
    }

    public async Task<List<ComplaintDto>> GetAllCustomerComplaints(Guid userId, int page)
    {
        var complaints = await _complaintRepository.GetAllUserComplaints(userId, page);

        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;
    }

    public async Task<List<ComplaintDto>> GetUnansweredCustomerComplaints(Guid userId, int page)
    {
        var complaints = await _complaintRepository.GetUnansweredUserComplaints(userId, page);

        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;
    }

    public async Task<List<ComplaintDto>> GetAnsweredCustomerComplaints(Guid userId, int page)
    {
        var complaints = await _complaintRepository.GetAnsweredUserComplaints(userId, page);

        var complaintDtos = _mapper.Map<List<ComplaintDto>>(complaints);

        return complaintDtos;
    }



    public async Task AnswerToComplaint(Guid complaintId, string answer, Guid adminId)
    {
        var complaint = await _complaintRepository.GetByIdAsync(complaintId) ?? throw new KeyNotFoundException();
        var compnayAdmin = await _userService.GetCompanyAdmin(adminId);

        complaint.AnswerComplaint(answer, compnayAdmin);

        await _unitOfWork.StartTransactionAsync();

        _complaintRepository.Update(complaint);
        await _httpClientService.ComplaintSender(complaint, answer);

        await _unitOfWork.SaveAndCommitChangesAsync();
    }


}
