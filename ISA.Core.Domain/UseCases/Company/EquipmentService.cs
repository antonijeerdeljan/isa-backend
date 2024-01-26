using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.Reservation;
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
    private readonly ICompanyAdminRepository _companyAdminRepository;

    public EquipmentService(IEquipmentRepository equipmentRepository, ICompanyRepository companyRepository, ICompanyAdminRepository companyAdminRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper, ICompanyService companyService, UserService userService)  : base(mapper)

    {
        _equipmentRepository = equipmentRepository;
        _companyRepository = companyRepository;
        _companyAdminRepository = companyAdminRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
        _companyService = companyService;
        _userService = userService;
    }

    public async Task AddAsync(string equipmentName, int quantity, Guid userId)
    {
        
        var companyAdmin = await _companyAdminRepository.GetByIdAsync(userId);
        Equipment equipment = new(equipmentName,quantity,companyAdmin.Company);
        if (companyAdmin.Company is not null)
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

    public async Task UpdateAsync(Guid id, string name, int quantity, Guid userId)
    {
        var companyAdmin = await _companyAdminRepository.GetByIdAsync(userId);
        Equipment newEquipment = new Equipment(id, name, quantity, companyAdmin.Company);
        _equipmentRepository.UpdateAndSaveChanges(newEquipment);
    }

    public async Task ReturnEqupment(IEnumerable<ReservationEquipment> equipment)
    {
        foreach (var equipmentItem in equipment)
        {
            var foundEqupment = await _equipmentRepository.GetByIdAsync(equipmentItem.EquipmentId) ?? throw new KeyNotFoundException();
            foundEqupment.ReturnEquipment(equipmentItem.Quantity);
            _equipmentRepository.Update(foundEqupment);
        }
    }
}





