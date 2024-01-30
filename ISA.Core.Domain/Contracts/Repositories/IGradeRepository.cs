namespace ISA.Core.Domain.Contracts.Repositories
{
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGradeRepository
    {
        Task AddAsync(Grade grade);
        Task<Grade?> GetByIdAsync(Guid id);
        void UpdateAndSaveChanges(Grade grade);
        void Update(Grade companyAdmin);

        Task<IEnumerable<Grade>> GetAllCompanyGrades(Guid companyId);

        Task<bool> CheckIfAlreadyExist(Guid userId, Guid companyId);
    }
}
