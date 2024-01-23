namespace ISA.Core.Domain.Contracts.Repositories
{
    using ISA.Core.Domain.Entities.Reservation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReservationEquipmentRepository
    {
        public Task AddAsync(ReservationEquipment reservationEquipment);
        public Task<ReservationEquipment?> GetByIdAsync(Guid id);
        public Task SaveAsync();
        public void Update(ReservationEquipment reservationEquipment);
    }
}
