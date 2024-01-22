using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Domain.UseCases.User;

namespace ISA.Core.Domain.UseCases.Company;

public class EquipmentService : BaseService<EquipmentDto, Equipment>, IEquipmentService
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompanyService _companyService;
    private readonly UserService _userService;

    public EquipmentService(IEquipmentRepository equipmentRepository, ICompanyRepository companyRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper, ICompanyService companyService, UserService userService)  : base(mapper)

    {
        _equipmentRepository = equipmentRepository;
        _companyRepository = companyRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
        _companyService = companyService;
        _userService = userService;
    }

    public async Task AddAsync(string equpmentName, int quantity, Guid companyId, Guid userId)
    {
        if(await _userService.IsUserIdInCompanyAdmins(userId,companyId) is false)
        {
            throw new ArgumentException();
        }
        var company = await _companyService.GetCompanyAsync(companyId);
        Equipment equipment = new(equpmentName,quantity,company);
        if (_companyRepository.Exist(equipment.Company.Id))
        {
            await _isaUnitOfWork.StartTransactionAsync();
            try
            {
                await _equipmentRepository.AddAsync(equipment);
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
            await _equipmentRepository.RemoveAndSaveChangesAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }

    public async Task UpdateAsync(Equipment newEquipment)
    {
        _equipmentRepository.UpdateAndSaveChanges(newEquipment);
    }
}





