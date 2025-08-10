using System.Threading.Tasks;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Infrastructure.Data;
using ClinicAppointmentManager.Infrastructure.Repositories;

namespace ClinicAppointmentManager.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicDbContext _context;

        public UnitOfWork(ClinicDbContext context)
        {
            _context = context;

            Doctors = new DoctorRepository(_context);
            Clinics = new ClinicRepository(_context);
            Patients = new PatientRepository(_context);
            Appointments = new AppointmentRepository(_context);
            Specialties = new SpecialtyRepository(_context);
            MedicalHistories = new Repository<MedicalHistory>(_context);

        }

        public IDoctorRepository Doctors { get; private set; }
        public IClinicRepository Clinics { get; private set; }
        public IPatientRepository Patients { get; private set; }
        public IAppointmentRepository Appointments { get; private set; }
        public ISpecialtyRepository Specialties { get; private set; }
        public IRepository<MedicalHistory> MedicalHistories { get; private set; }


        public async Task<int> CompleteAsync()
        {

            return await _context.SaveChangesAsync();
        }
       

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
