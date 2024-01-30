namespace ISA.Core.Domain.UseCases.Company
{
    using AutoMapper;
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Contracts.Services;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Dtos.Company;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.Reservation;
    using ISA.Core.Domain.Entities.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GradeService : BaseService<GradeDto, Grade>, IGradeService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IISAUnitOfWork _isaUnitOfWork;

        public GradeService(ICompanyRepository companyRepository, ICustomerRepository customerRepository, IGradeRepository gradeRepository, IReservationRepository reservationRepository, IMapper mapper, IISAUnitOfWork isaUnitOfWork) : base(mapper)
        {
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
            _gradeRepository = gradeRepository;
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _isaUnitOfWork = isaUnitOfWork;
        }

        public async Task AddAsync(Guid companyId, Guid userId, int rate, string reason, string text)
        {
            if (await CanRate(userId, companyId) is false) throw new KeyNotFoundException();
            if (await CheckIfAlreadyExist(userId, companyId) is true) throw new KeyNotFoundException();   
            await _isaUnitOfWork.StartTransactionAsync();
            Grade newGrade = new Grade(userId, companyId, rate, reason, text);
            try
            {
                await _gradeRepository.AddAsync(newGrade);
                await _isaUnitOfWork.SaveAndCommitChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            

        }

        public async Task<bool> CheckIfAlreadyExist(Guid userId, Guid companyId)
        {
            return await _gradeRepository.CheckIfAlreadyExist(userId, companyId);
        }

        public async Task<bool> CanRate(Guid userId, Guid companyId)
        {
            var reservations = await _reservationRepository.GetHistoryOfCustomerReservations(userId);
            return (reservations.Where(r => r.Appointment.Company.Id == companyId).Count() > 0);
        }

        public async Task<IEnumerable<GradeDto>> GetAllCompanyGrades(Guid companyId)
        {
            var grades = await _gradeRepository.GetAllCompanyGrades(companyId);
            var gradesDto = grades.Select(grade => _mapper.Map<GradeDto>(grade));
            return gradesDto;
        }

        public async Task<Grade?> GetByIdAsync(Guid id)
        {
            return await _gradeRepository.GetByIdAsync(id);
        }

        public async Task Update(Guid userId,Guid id, int rate, string reason, string text)
        {
            var grade = await _gradeRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
            if (userId != grade.CustomerUserId) throw new KeyNotFoundException();
            if (rate < 1 || rate > 10) throw new KeyNotFoundException();
            await _isaUnitOfWork.StartTransactionAsync();
            grade.Rate = rate; grade.Text = text; grade.Reason = reason;
            _gradeRepository.UpdateAndSaveChanges(grade);
            await _isaUnitOfWork.SaveAndCommitChangesAsync();
        }
    }
}
