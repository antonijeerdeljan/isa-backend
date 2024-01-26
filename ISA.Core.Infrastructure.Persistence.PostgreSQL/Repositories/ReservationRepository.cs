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
        try
        {
            return await _dbSet.Include(c => c.Customer)
                               .Include(e => e.Equipments)
                               .Include(a => a.Appointment)
                               .Where(c => c.State == ReservationState.Pending && c.Appointment.EndingDateTime < DateTime.UtcNow)
                               .ToListAsync();
        }
        catch (Exception ex)
        {
            // Log the exception details for debugging purposes
            // You can use a logging framework/library of your choice
            Console.WriteLine(ex.ToString(), "An error occurred while checking for overdue reservations.");

            // Optionally, you might want to throw the exception,
            // return an empty list, or handle it in some other way
            // depending on your application's needs.
            return new List<Reservation>();
            // or throw; to rethrow the original exception
        }
    }





}
