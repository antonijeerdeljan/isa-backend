namespace ISA.Core.Domain.Contracts.Services
{
    using ISA.Core.Domain.Dtos.Company;
    using ISA.Core.Domain.Entities.Company;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGradeService
    {
        Task AddAsync(Guid companyId, Guid userId, int rate, string reason, string text);
        Task<Grade?> GetByIdAsync(Guid id);
        Task Update(Guid userId, Guid id, int rate, string reason, string text);
        Task<IEnumerable<GradeDto>> GetAllCompanyGrades(Guid companyId);
        Task<bool> CheckIfAlreadyExist(Guid userId, Guid companyId);
        Task<bool> CanRate(Guid userId, Guid companyId);
    }
}
