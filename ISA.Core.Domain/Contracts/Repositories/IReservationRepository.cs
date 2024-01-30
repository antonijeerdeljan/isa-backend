using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
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
    Task<List<Reservation>> GetAllCustomerReservations(Guid userId);
    Task<List<Reservation>> GetAllScheduledCustomerReservations(Guid userId);

    Task<List<Reservation>> GetHistoryOfCustomerReservations(Guid customerId);

    Task<IEnumerable<Reservation>> GetAllCompanyCustomers(Guid companyId);


    Task<bool> EquipmentCanBeDeleted(Guid id);

}
