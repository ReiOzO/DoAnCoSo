using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Persistence.Repositories;

namespace BELibrary.Core.Entity
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HospitalManagementDbContext _context;

        public UnitOfWork(HospitalManagementDbContext context)
        {
            _context = context;
            Accounts = new AccountRepository(_context);
            Categories = new CategoryRepository(_context);
            Services = new ServiceRepository(_context);
            DetailRecords = new DetailRecordRepository(_context);
            Patients = new PatientRepository(_context);
            Records = new RecordRepository(_context);
            Doctors = new DoctorRepository(_context);
            Faculties = new FacultyRepository(_context);
            Faculties = new FacultyRepository(_context);
            PatientRecords = new PatientRecordRepository(_context);
            UserVerifications = new UserVerificationRepository(_context);
            DoctorSchedules = new DoctorScheduleRepository(_context);
            PatientDoctors = new PatientDoctorRepository(_context);
            Articles = new ArticleRepository(_context);
        }

        public IAccountRepository Accounts { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IServiceRepository Services { get; private set; }
        public IDetailRecordRepository DetailRecords { get; private set; }
        public IPatientRepository Patients { get; private set; }
        public IRecordRepository Records { get; private set; }
        public IFacultyRepository Faculties { get; private set; }
        public IDoctorRepository Doctors { get; private set; }
        public IPatientRecordRepository PatientRecords { get; private set; }
        public IUserVerificationRepository UserVerifications { get; private set; }
        public IDoctorScheduleRepository DoctorSchedules { get; private set; }
        public IPatientDoctorRepository PatientDoctors { get; private set; }
        public IArticleRepository Articles { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}