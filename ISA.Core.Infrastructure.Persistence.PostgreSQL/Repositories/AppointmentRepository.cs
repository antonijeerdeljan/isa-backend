namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories
{
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Entities;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Infrastructure.Persistence.PostgreSQL.QueryExtensionMethods;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AppointmentRepository : GenericRepository<Appointment, Guid>, IAppointmentRepository
    {
        private readonly IsaDbContext _isaDbContext;
        public AppointmentRepository(IsaDbContext isaDbContext) : base(isaDbContext)
        {
            _isaDbContext = isaDbContext;
        }

        public override async Task<Appointment?> GetByIdAsync(Guid appointmentId)
        {
            return await _dbSet.Include(c => c.Company)
                               .Include(c => c.CompanyAdmin)
                               .Where(c => c.Id == appointmentId)
                               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllCompanyAppointments(int page, Guid companyId)
        {
            return await _dbSet.GetPaged(page).Where(a => a.Company.Id == companyId).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllCompanyAppointmentsForDate(int page, Guid companyId, DateOnly date)
        {
            return await _dbSet.GetPaged(page).Where(a => a.Company.Id == companyId && a.StartingDateTime.Date.DayOfYear == date.DayOfYear).ToListAsync();
        }
    }
}
