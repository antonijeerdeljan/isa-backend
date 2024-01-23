namespace ISA.Core.Domain.Contracts.Repositories
{
    using ISA.Core.Domain.Entities.Company;
    using System;
    using System.Threading.Tasks;

    public interface IEquipmentRepository
    {
        Task AddAsync(Equipment equipmentToAdd);

        Task RemoveAsync(Guid id);
        Task<Equipment?> GetByIdAsync(Guid id);

        void UpdateAndSaveChanges(Equipment equipment);

        void Update(Equipment equipment);

        Task RemoveAndSaveChangesAsync(Guid Id);

        Task<bool> ExistEnough(Guid id, int quantity);
    }
}
