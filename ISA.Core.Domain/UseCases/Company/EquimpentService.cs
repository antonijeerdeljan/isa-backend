namespace ISA.Core.Domain.UseCases.Company
{
    using AutoMapper;
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Contracts.Services;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Entities.Company;
    using System;
    using System.Threading.Tasks;

    public class EquimpentService
    {
        private readonly IEquipmentRepository _equimpentRepository;
        private readonly ICompanyRepository _newEquipmentRepository;
        private readonly IISAUnitOfWork _isaUnitOfWork;
        private readonly IMapper _mapper;

        public EquimpentService(IEquipmentRepository equimpentRepository, ICompanyRepository newEquipmentRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper)
        {
            _equimpentRepository = equimpentRepository;
            _newEquipmentRepository = newEquipmentRepository;
            _isaUnitOfWork = isaUnitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(Equipment equipment)
        {
            if (_newEquipmentRepository.Exist(equipment.Company.Id))
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

        public async Task RemoveAsync(Guid id)
        {
            await _isaUnitOfWork.StartTransactionAsync();
            try
            {
                await _equimpentRepository.RemoveAndSaveChangesAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public async Task UpdateAsync(Equipment newEquipment)
        {
            _equimpentRepository.UpdateAndSaveChanges(newEquipment);
        }
    }
}





