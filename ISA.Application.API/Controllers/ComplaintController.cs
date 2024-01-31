using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Dtos.Admin;
using ISA.Core.Domain.Dtos.Company;
using ISA.Core.Domain.Entities.Complaint;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Domain.UseCases.Complaint;
using ISA.Core.Domain.UseCases.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISA.Application.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ComplaintController : ControllerBase
{
    private readonly ComplaintService _complaintService;
    public ComplaintController(ComplaintService complaintService)
    {
        _complaintService = complaintService;
    }

    [HttpPost("create-admin-complaint")]
    [Authorize(Policy = "customerPolicy")]
    public async Task AddAdminComplaint([FromBody] AddComplaintModel complaint)
    {
        Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        await _complaintService.CreateAdminComplaint(userId, complaint.ComplaintObjectId, complaint.Title, complaint.Description);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("create-compnay-complaint")]
    [Authorize(Policy = "customerPolicy")]
    public async Task AddCompanyComplaint([FromBody] AddComplaintModel complaint)
    {
        Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        await _complaintService.CreateAdminComplaint(userId, complaint.ComplaintObjectId, complaint.Title, complaint.Description);
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("complaint-target-admins")]
    [Authorize(Policy = "customerPolicy")]
    public async Task<List<CompanyAdminDto>> GetPossibleAdmins()
    {
        Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetPossibleComplaintAdmins(userId);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("complaint-target-compnay")]
    [Authorize(Policy = "customerPolicy")]
    public async Task<List<CompanyBasicInfoDto>> GetPossibleCompanies()
    {
        Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetPossibleComplaintCompanies(userId);

    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("complaints-on-company")]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task<List<Complaint>> GetAllComplaintsOnCompnay(int page)
    {
        Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetAllCompanyComplaints(adminId, page);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("complaints-on-company-unanswered")]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task<List<Complaint>> GetAllComplaintsOnCompnayUnAnswered(int page)
    {
        Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetUnansweredCompanyComplaints(adminId, page);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("complaints-on-company-answered")]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task<List<Complaint>> GetAllComplaintsOnCompnayAnswered(int page)
    {
        Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetAnsweredCompanyComplaints(adminId, page);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("complaints-on-admin")]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task<List<Complaint>> GetAllComplaintsOnAdmin(int page)
    {
        Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetAllAdminComplaints(adminId, page);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("complaints-on-admin-unanswered")]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task<List<Complaint>> GetAllComplaintsOnAdminUnAnswered(int page)
    {
        Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetUnansweredAdminComplaints(adminId, page);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("complaints-on-admin-answered")]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task<List<Complaint>> GetAllComplaintsOnAdminAnswered(int page)
    {
        Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetAnsweredAdminComplaints(adminId, page);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("sent-complaints")]
    [Authorize(Policy = "customerPolicy")]
    public async Task<List<Complaint>> GetAllCustomerComplaints(int page)
    {
        Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetAllCustomerComplaints(userId, page);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("unanswered-complaints")]
    [Authorize(Policy = "customerPolicy")]
    public async Task<List<Complaint>> GetUnansweredCustomerComplaints(int page)
    {
        Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetUnansweredCustomerComplaints(userId, page);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("answered-complaints")]
    [Authorize(Policy = "customerPolicy")]
    public async Task<List<Complaint>> GetAnsweredCustomerComplaints(int page)
    {
        Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        return await _complaintService.GetAnsweredCustomerComplaints(userId, page);
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("answer-complaint")]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task ComplaintAnswer(ComplaintAnswerRequest complaintAnswer)
    {
        Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        await _complaintService.AnswerToComplaint(complaintAnswer.complaintId, complaintAnswer.answer, adminId);
    }























}