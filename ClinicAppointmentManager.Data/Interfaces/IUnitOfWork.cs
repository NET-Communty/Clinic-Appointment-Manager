using ClinicAppointmentManager.Core.Entities;

namespace ClinicAppointmentManager.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDoctorRepository Doctors { get; }
        IPatientRepository Patients { get; }
        IAppointmentRepository Appointments { get; }
        IClinicRepository Clinics { get; }
        ISpecialtyRepository Specialties { get; }
        IRepository<MedicalHistory> MedicalHistories { get; }

        Task<int> CompleteAsync();
    }
}