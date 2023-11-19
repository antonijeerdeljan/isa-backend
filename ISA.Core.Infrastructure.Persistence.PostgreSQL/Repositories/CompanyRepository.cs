namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories
{
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Entities.Company;
    using System;

    public class CompanyRepository : GenericRepository<Company, Guid>, ICompanyRepository
    {
        private readonly IsaDbContext _isaDbContext;
        public CompanyRepository(IsaDbContext isaDbContext) : base(isaDbContext)
        {
            _isaDbContext = isaDbContext;
        }

    }
}
