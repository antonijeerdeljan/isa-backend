using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.Complaint;
using ISA.Core.Infrastructure.Persistence.PostgreSQL.QueryExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class ComplaintRepository : GenericRepository<Complaint, Guid>, IComplaintRepository
{
    public ComplaintRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {
    }

    public async Task<List<Complaint>> GetAllCompanyComplaints(Guid companyId, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.SubjectComplaintId == companyId && c.Type == ComplaintType.Compnay).ToListAsync();
    }

    public async Task<List<Complaint>> GetUnansweredCompanyComplaints(Guid companyId,int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.SubjectComplaintId == companyId && c.Resloved == false && c.Type == ComplaintType.Compnay).ToListAsync();
    }

    public async Task<List<Complaint>> GetAnsweredCompanyComplaints(Guid companyId, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.SubjectComplaintId == companyId && c.Resloved == true && c.Type == ComplaintType.Compnay).ToListAsync();
    }






    public async Task<List<Complaint>> GetAllAdminComplaints(Guid adminId, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.SubjectComplaintId == adminId && c.Type == ComplaintType.Admin).ToListAsync();
    }

    public async Task<List<Complaint>> GetUnansweredAdminComplaints(Guid adminId, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.SubjectComplaintId == adminId && c.Resloved == false && c.Type == ComplaintType.Admin).ToListAsync();
    }

    public async Task<List<Complaint>> GetAnsweredAdminComplaints(Guid adminId, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.SubjectComplaintId == adminId && c.Resloved == true && c.Type == ComplaintType.Admin).ToListAsync();
    }




    public async Task<List<Complaint>> GetAllUserComplaints(Guid userId, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.Customer.UserId == userId).ToListAsync();
    }

    public async Task<List<Complaint>> GetUnansweredUserComplaints(Guid userId, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.Customer.UserId == userId && c.Resloved == false).ToListAsync();
    }

    public async Task<List<Complaint>> GetAnsweredUserComplaints(Guid userId, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.Customer.UserId == userId && c.Resloved == true).ToListAsync();
    }


    public override async Task<Complaint?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Include(c => c.Customer).Include(c => c.AnsweredBy.User).Where(c => c.Id == id).FirstOrDefaultAsync();
    }





}
