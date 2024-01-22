using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;

namespace ISA.Core.Domain.UseCases.Company;

public class EquipmentService : BaseService<EquipmentDto, Equipment>, IEquipmentService
{
    private readonly IEquipmentRepository _equimpentRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompanyService _companyService;

    public EquipmentService(IEquipmentRepository equimpentRepository, ICompanyRepository companyRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper, ICompanyService companyService)  : base(mapper)
    {
        _equimpentRepository = equimpentRepository;
        _companyRepository = companyRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
        _companyService = companyService;
    }

    public async Task AddAsync(string equpmentName, int quantity, Guid CompanyId)
    {
        var company = await _companyService.GetCompanyAsync(CompanyId);
        Equipment equipment = new(equpmentName,quantity,company);
        if (_companyRepository.Exist(equipment.Company.Id))
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





