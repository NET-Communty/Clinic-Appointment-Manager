using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentManager.Core.Entities
{
    public class Doctor : BaseEntity
    {
        public int DoctorId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string LicenseNumber { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public int SpecialtyId { get; set; }
        public virtual Specialty Specialty { get; set; } = null!;

        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; } = null!;
    }
}
