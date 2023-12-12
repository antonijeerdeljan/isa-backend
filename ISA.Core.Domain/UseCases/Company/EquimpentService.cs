namespace ISA.Core.Domain.UseCases.Company
{
    using AutoMapper;
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Contracts;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Entities.Company;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EquimpentService
    {
        private readonly IEquipmentRepository _equimpentRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IISAUnitOfWork _isaUnitOfWork;
        private readonly IMapper _mapper;

        public EquimpentService(IEquipmentRepository equimpentRepository, ICompanyRepository companyRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper)
        {
            _equimpentRepository = equimpentRepository;
            _companyRepository = companyRepository;
            _isaUnitOfWork = isaUnitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(Equipment equipment)
        {
            if (_companyRepository.Exist(equipment.CompanyId))
            {
                await _isaUnitOfWork.StartTransactionAsync();
                try
                {
                    await _equimpentRepository.AddAsync(equipment);
                    await _isaUnitOfWork.SaveAndCommitChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

        }
    }
}





