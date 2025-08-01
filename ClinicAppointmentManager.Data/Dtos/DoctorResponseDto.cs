using ClinicAppointmentManager.Core.Entities;

namespace ClinicAppointmentManager.Core.Dtos
{
    public class DoctorResponseDto
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string SpecialtyName { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty   ;
    }
}
