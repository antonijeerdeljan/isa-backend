﻿namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories
{
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Contracts.Services;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Infrastructure.Persistence.PostgreSQL.QueryExtensionMethods;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    public class EquipmentRepository : GenericRepository<Equipment, Guid>, IEquipmentRepository
    {
        private readonly IsaDbContext _isaDbContext;
        public EquipmentRepository(IsaDbContext isaDbContext) : base(isaDbContext)
        {
            _isaDbContext = isaDbContext;
        }

        public async Task EquipmentSold(Guid id, int quantity)
        {
            
            var equipment = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            equipment.Quantity -= quantity;
            _dbSet.Update(equipment);
            
        }

        public async Task <bool> ExistEnough(Guid id, int quantity)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && e.Quantity >= quantity) is not null;
        }

        public async Task<List<Equipment>> GetGeneralEquipment(int page)
        {
            return await _dbSet.Where(c => c.Quantity == null && c.CompanyId == null)
                               .GetPaged(page)
                               .ToListAsync();
        }

        public async Task DeleteContract(Guid id)
        {
            var equipment = await _dbSet.FirstOrDefaultAsync(c => c.Id == id);
            _dbSet.Remove(equipment);
        }


        public new async Task<Equipment?> GetByIdAsync(Guid id)
        {
            return await _dbSet.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
    }
}
