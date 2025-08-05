namespace ClinicAppointmentManager.Core.Dtos.Doctor
{
    public class DoctorScheduleDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public List<OperatingHourDto> OperatingHours { get; set; } = new();
    }

    public class OperatingHourDto
    {
        public string DayOfWeek { get; set; } = string.Empty;
        public string OpenTime { get; set; } = string.Empty;
        public string CloseTime { get; set; } = string.Empty;
    }
}
