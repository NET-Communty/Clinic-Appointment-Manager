using ClinicAppointmentManager.Core.Enums;

namespace ClinicAppointmentManager.Core.Dtos.Doctor
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public enAppointmentStatus Status { get; set; }
    }
}
