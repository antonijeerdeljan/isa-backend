using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.Reservation;
using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Linq;

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

    public async Task<bool> EquipmentCanBeDeleted(Guid id)
    {
        bool e =  _dbSet.Include(r => r.Appointment)
                     .Include(r => r.Equipments)
                     .Where(r => r.State == ReservationState.Pending && r.Equipments.Any(r => r.EquipmentId == id))
                     .Count() == 0;
        return e;             
    }

    public async Task<List<Reservation>> GetAllCompanyReservations(Guid companyId)
    {
        return await _dbSet.Include(r => r.Appointment)
                            .Include(r => r.Equipments).ThenInclude(e => e.Equipment)
                            .Where(r => r.Appointment.Company.Id == companyId)
                            .ToListAsync();
    }

    public async Task<List<Reservation>> GetAllCustomerReservations(Guid userId)
    {
        return await _dbSet.Include(r => r.Appointment)
                            .Include(r => r.Equipments).ThenInclude(e => e.Equipment)
                            .Where(r => r.Customer.UserId == userId)
                            .ToListAsync();
    }

    public async Task<List<Reservation>> GetAllScheduledCustomerReservations(Guid userId)
    {
        return await _dbSet.Include(r => r.Appointment)
                            .Include(r => r.Equipments).ThenInclude(e => e.Equipment)
                            .Where(r => r.Customer.UserId == userId && r.State == ReservationState.Pending)
                            .ToListAsync();
    }

    public override async Task<Reservation?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.Include(c => c.Customer)
                           .Include(c => c.Appointment).ThenInclude(a => a.CompanyAdmin)
                           .Include(c => c.Equipments).ThenInclude(e => e.Equipment)
                           .Where(i => i.AppointmentId == Id)
                           .FirstOrDefaultAsync();
    }

    public async Task<List<Reservation>> GetHistoryOfCustomerReservations(Guid customerId)
    {
        return await _dbSet.Include(r => r.Appointment).Where(a => a.State != ReservationState.Pending)
                            .Include(r => r.Equipments).ThenInclude(e => e.Equipment)
                            .Where(r => r.Customer.UserId == customerId)
                            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllCompanyCustomers(Guid companyId)
    {
        return await _dbSet.Where(a => a.Appointment.Company.Id == companyId).Include(a => a.Customer).ThenInclude(c => c.User).ThenInclude(u => u.Address).ToListAsync();
    }
}
