using ClinicAppointmentManager.Core.Enums;

namespace ClinicAppointmentManager.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public enAppointmentStatus Status { get; set; }
        public string? Notes { get; set; } = null;

        // Foreign keys
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        // Navigation properties
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;

    }

}
