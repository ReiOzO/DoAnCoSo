namespace BELibrary.DbContext
{
    using BELibrary.Entity;
    using System.Data.Entity;

    public partial class HospitalManagementDbContext : DbContext
    {
        public HospitalManagementDbContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<DetailRecord> DetailRecords { get; set; }

        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Record> Records { get; set; }

        public virtual DbSet<Faculty> Faculties { get; set; }

        public virtual DbSet<Doctor> Doctors { get; set; }

        public virtual DbSet<PatientRecord> PatientRecords { get; set; }

        public virtual DbSet<UserVerification> UserVerifications { get; set; }

        public virtual DbSet<DoctorSchedule> DoctorSchedules { get; set; }

        public virtual DbSet<PatientDoctor> PatientDoctors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Patient>()
                .Property(e => e.IndentificationCardId)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            /*modelBuilder.Entity<Patient>()
                .HasMany(e => e.MedicalSupplies)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);*/

            modelBuilder.Entity<Record>()
                .HasMany(e => e.DetailRecords)
                .WithRequired(e => e.Record)
                .WillCascadeOnDelete(false);
        }
    }
}