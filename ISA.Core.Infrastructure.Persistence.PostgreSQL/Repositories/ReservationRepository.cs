using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities;
using ISA.Core.Domain.Entities.Reservation;
using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class ReservationRepository : GenericRepository<Reservation, Guid>, IReservationRepository
{
    
    public ReservationRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {

    }


    public async Task<List<Reservation>> CheckForOverdueReservations()
    {

            return await _dbSet.Include(c => c.Customer)
                               .Include(e => e.Equipments)
                               .Include(a => a.Appointment)
                               .Where(c => c.State == ReservationState.Pending && c.Appointment.EndingDateTime < DateTime.UtcNow)
                               .ToListAsync();
        
    }

    public async Task<List<Reservation>> GetAllCompanyReservations(Guid companyId)
    {
        return await _dbSet.Include(r => r.Appointment)
                            .Include(r => r.Equipments)
                            .Where(r => r.Appointment.Company.Id == companyId)
                            .ToListAsync();
    }

    public override async Task<Reservation?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.Include(c => c.Customer)
                           .Include(c => c.Appointment)
                           .Include(c => c.Equipments)
                           .Where(i => i.AppointmentId == Id)
                           .FirstOrDefaultAsync();
    }





}
