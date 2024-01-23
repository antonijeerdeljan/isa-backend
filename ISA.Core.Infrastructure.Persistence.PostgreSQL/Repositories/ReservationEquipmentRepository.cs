namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories
{
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.Reservation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ReservationEquipmentRepository : GenericRepository<ReservationEquipment, Guid>, IReservationEquipmentRepository
    {
        private readonly IsaDbContext _isaDbContext;
        public ReservationEquipmentRepository(IsaDbContext isaDbContext) : base(isaDbContext)
        {
            _isaDbContext = isaDbContext;
        }
    }
}
