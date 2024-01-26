namespace ISA.Core.Domain.Contracts.Services
{
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.Reservation;
    using System;
    using System.Threading.Tasks;

    public interface IEquipmentService
    {
        Task AddAsync(string equpmentName, int quantity, Guid userId);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Guid id, string name, int quantity, Guid userId);
        Task ReturnEqupment(IEnumerable<ReservationEquipment> equipment);
    }
}
