using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicAppointmentManager.Core.Dtos.MedicalHistory;
using ClinicAppointmentManager.Core.Entities;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);

        Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId);
        Task<IEnumerable<Doctor>> GetPatientDoctorsAsync(int patientId);
        Task RequestAppointmentAsync(int patientId, int doctorId, DateTime appointmentDate, int durationMinutes);
        Task<IEnumerable<MedicalHistoryDto>> GetPatientMedicalHistoryAsync(int patientId);
        Task AddMedicalHistoryAsync(int patientId, MedicalHistoryCreateDto dto);

    }
}
