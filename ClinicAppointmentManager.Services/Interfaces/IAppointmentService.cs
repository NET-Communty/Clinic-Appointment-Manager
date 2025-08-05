using ClinicAppointmentManager.Core.Dtos.Appointment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
        Task<AppointmentResponseDto?> GetByIdAsync(int id);
        Task<AppointmentResponseDto> AddAsync(AppointmentPostDto dto);
        Task<bool> CheckAvailabilityAsync(int doctorId, DateTime appointmentDate);
        Task RescheduleAsync(int appointmentId, DateTime newDate);
        Task CancelAsync(int appointmentId);
        Task<IEnumerable<AppointmentResponseDto>> GetUpcomingAppointmentsAsync(int? doctorId, int? patientId);

        Task UpdateAsync(AppointmentPutDto dto);
        Task DeleteAsync(int id);



    }
}
