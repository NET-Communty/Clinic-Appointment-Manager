namespace ClinicAppointmentManager.Core.Dtos
{
    public class ClinicResponseDto
    {
        public int ClinicId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
