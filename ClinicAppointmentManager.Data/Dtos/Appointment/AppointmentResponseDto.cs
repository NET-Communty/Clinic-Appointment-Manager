using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Core.Dtos.Appointment
{
    public class AppointmentResponseDto
    {


        public int AppointmentId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
