namespace ISA.Core.Domain.Contracts.Repositories
{
    using ISA.Core.Domain.Entities.Company;
    using System;
    using System.Threading.Tasks;

    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(Guid id);

        void UpdateAndSaveChanges(Appointment appointment);

        void Update(Appointment appointment);

        Task<IEnumerable<Appointment>> GetAllCompanyAppointments(int page, Guid companyId);
    }
}
