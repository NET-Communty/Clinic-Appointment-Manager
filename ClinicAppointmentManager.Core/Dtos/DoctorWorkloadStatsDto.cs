namespace ClinicAppointmentManager.Data.Dtos
{
    public class DoctorWorkloadStatsDto
    {
        public int DoctorId { get; set; }
        public int TotalAppointments { get; set; }
        public double TotalHoursWorked { get; set; }
        public int Cancellations { get; set; }
    }
}
