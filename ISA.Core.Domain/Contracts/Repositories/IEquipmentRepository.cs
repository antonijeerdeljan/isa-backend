namespace ISA.Core.Domain.Contracts.Repositories
{
    using ISA.Core.Domain.Entities.Company;
    using System;
    using System.Threading.Tasks;

    public interface IEquipmentRepository
    {
        Task AddAsync(Equipment equipmentToAdd);
        Task<Equipment?> GetByIdAsync(Guid id);

        void UpdateAndSaveChanges(Equipment equipment);

        void Update(Equipment equipment);
    }
}
