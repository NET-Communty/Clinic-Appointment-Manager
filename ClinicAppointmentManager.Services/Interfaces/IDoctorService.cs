using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ClinicAppointmentManager.Core.Dtos.Doctor;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponseDto>> GetAllAsync(int page = 1);
        Task<DoctorResponseDto?> GetByIdAsync(int id);
        Task<DoctorResponseDto> AddAsync(DoctorPostDto doctor);
        Task UpdateAsync(int id,DoctorPutDto doctor);
        Task DeleteAsync(int id);

        Task<DoctorScheduleDto> GetDoctorSchedule(int doctorId); // Get doctor's schedule with appointments

        Task<IEnumerable<DoctorResponseDto>> GetBySpecialtyAsync(int specialtyId);
        Task<IEnumerable<DoctorResponseDto>> GetByClinicAsync(int clinicId);

        Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(int doctorId, DateTime? fromDate, DateTime? toDate);

        Task<bool> IsAvailableAsync(int doctorId, DateTime desiredDateTime, int durationMinutes);

        
        Task<IEnumerable<AppointmentDto>> GetDailyScheduleAsync(int doctorId, DateTime date);// all appointments for a doctor on a specific day

        // Get doctor workload stats (total appointments, hours worked, cancellations)
        Task<DoctorWorkloadStatsDto> GetWorkloadStatsAsync(int doctorId, DateTime? fromDate, DateTime? toDate);

    }
}
