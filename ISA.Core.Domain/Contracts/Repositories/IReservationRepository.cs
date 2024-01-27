using ISA.Core.Domain.Entities.Reservation;

namespace ISA.Core.Domain.Contracts.Repositories;

public interface IReservationRepository
{
    public Task AddAsync(Reservation reservation);
    public Task<Reservation?> GetByIdAsync(Guid id);
    public Task SaveAsync();
    public void Update(Reservation reservation);
    Task<List<Reservation>> CheckForOverdueReservations();

    Task<List<Reservation>> GetAllCompanyReservations(Guid companyId);

}
