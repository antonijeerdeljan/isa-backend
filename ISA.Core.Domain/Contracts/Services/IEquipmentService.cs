namespace ISA.Core.Domain.Contracts.Services
{
    using ISA.Core.Domain.Entities.Company;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEquipmentService
    {
        Task AddAsync(string equpmentName, int quantity, Guid CompanyId);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Equipment newEquipment);
    }
}
