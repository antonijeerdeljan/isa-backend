using ISA.Core.Domain.Entities.Complaint;
using ISA.Core.Domain.Entities.LoyaltyProgram;

namespace ISA.Core.Domain.Contracts.Repositories;

public interface IComplaintRepository
{
    Task AddAsync(Complaint complaint);
    Task<Complaint?> GetByIdAsync(Guid id);
    void UpdateAndSaveChanges(Complaint complaint);
    void Update(Complaint complaint);

    Task<List<Complaint>> GetAllCompanyComplaints(Guid companyId, int page);

    Task<List<Complaint>> GetUnansweredCompanyComplaints(Guid companyId, int page);

    Task<List<Complaint>> GetAnsweredCompanyComplaints(Guid companyId, int page);

    Task<List<Complaint>> GetAllAdminComplaints(Guid adminId, int page);

    Task<List<Complaint>> GetUnansweredAdminComplaints(Guid adminId, int page);

    Task<List<Complaint>> GetAnsweredAdminComplaints(Guid adminId, int page);

    Task<List<Complaint>> GetAllUserComplaints(Guid userId, int page);

    Task<List<Complaint>> GetUnansweredUserComplaints(Guid userId, int page);

    Task<List<Complaint>> GetAnsweredUserComplaints(Guid userId, int page);

}
