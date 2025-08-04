namespace ClinicAppointmentManager.Core.Dtos.Specialty
{
    public class SpecialtyResponseDto
    {
        public int SpecialtyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
