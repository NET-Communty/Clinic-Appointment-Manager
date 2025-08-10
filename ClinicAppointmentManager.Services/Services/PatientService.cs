using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicAppointmentManager.Core.Dtos.MedicalHistory;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;

namespace ClinicAppointmentManager.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
            => await _unitOfWork.Patients.GetAllAsync();

        public async Task<Patient?> GetByIdAsync(int id)
            => await _unitOfWork.Patients.GetByIdAsync(id);

        public async Task AddAsync(Patient patient)
        {
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            await _unitOfWork.Patients.UpdateAsync(patient);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Patients.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            return await _unitOfWork.Appointments.FindAsync(a => a.PatientId == patientId);
        }

        public async Task<IEnumerable<Doctor>> GetPatientDoctorsAsync(int patientId)
        {
            var appointments = await _unitOfWork.Appointments.FindAsync(a => a.PatientId == patientId);
            var doctors = new List<Doctor>();

            foreach (var appt in appointments)
            {
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(appt.DoctorId);
                if (doctor != null && !doctors.Contains(doctor))
                    doctors.Add(doctor);
            }

            return doctors;
        }

        public async Task<IEnumerable<MedicalHistoryDto>> GetPatientMedicalHistoryAsync(int patientId)
        {
            var histories = await _unitOfWork.MedicalHistories.FindAsync(h => h.PatientId == patientId);
            return histories.Select(h => new MedicalHistoryDto
            {
                MedicalHistoryId = h.MedicalHistoryId,
                PatientId = h.PatientId,
                Diagnosis = h.Diagnosis,
                Treatment = h.Treatment,
                RecordDate = h.RecordDate
            });
        }

        public async Task AddMedicalHistoryAsync(int patientId, MedicalHistoryCreateDto dto)
        {
            var history = new MedicalHistory
            {
                PatientId = patientId,
                Diagnosis = dto.Diagnosis,
                Treatment = dto.Treatment,
                RecordDate = dto.RecordDate
            };

            await _unitOfWork.MedicalHistories.AddAsync(history);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RequestAppointmentAsync(int patientId, int doctorId, DateTime appointmentDate, int durationMinutes)
        {
            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = appointmentDate,
                DurationMinutes = durationMinutes,
                Status = Core.Enums.enAppointmentStatus.Scheduled
            };

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }
    }
}
