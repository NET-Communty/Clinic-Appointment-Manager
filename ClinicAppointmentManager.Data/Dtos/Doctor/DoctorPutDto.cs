namespace ClinicAppointmentManager.Core.Dtos.Doctor
{
    public class DoctorPutDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public int SpecialtyId { get; set; }
        public int ClinicId { get; set; }
    }
}
