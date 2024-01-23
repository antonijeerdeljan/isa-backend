using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.Reservation;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class ReservationRepository : GenericRepository<Reservation, Guid>, IReservationRepository
{
    public ReservationRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {

    }
}
