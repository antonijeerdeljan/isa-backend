﻿namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories
{
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Entities;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;
    using ISA.Core.Infrastructure.Persistence.PostgreSQL.QueryExtensionMethods;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class CompanyRepository : GenericRepository<Company, Guid>, ICompanyRepository
    {
        private readonly IsaDbContext _isaDbContext;
        public CompanyRepository(IsaDbContext isaDbContext) : base(isaDbContext)
        {
            _isaDbContext = isaDbContext;
        }

        public bool Exist(Guid id)
        {
            var company = _dbSet.Where(c => c.Id == id).FirstOrDefault();
            if (company == null)
            {
                return false;
            }
            return true;
        }

        new public async Task<Company?> GetByIdAsync(Guid Id)
        {
            return await _dbSet.Where(t => t.Id == Id).Include(t => t.Address).Include(t => t.Admins).Include(t => t.Equipment).FirstOrDefaultAsync(); ;
        }


        public async Task<IEnumerable<Company>> GetAllCompanies(int page)
        {
            return await _dbSet.GetPaged(page).Include(c => c.Address).Include(c => c.Equipment).Include(c => c.Appointments).ToListAsync();
        }

        public async Task<bool> IsAppointmentInWorkingHours(DateTime start, DateTime end, Guid id)
        {
            var company = await _dbSet.FirstOrDefaultAsync(c => c.Id == id);
            return (start.TimeOfDay >= company.StartingWorkingHour.ToTimeSpan()  &&
                    end.TimeOfDay <= company.EndWorkingHour.ToTimeSpan() && 
                    end >= start && start >= DateTime.UtcNow.AddHours(1) && 
                    start.Date == end.Date && start.Date.DayOfWeek != DayOfWeek.Sunday && 
                    start.Date.DayOfWeek != DayOfWeek.Saturday) ;

        }
    }
}
