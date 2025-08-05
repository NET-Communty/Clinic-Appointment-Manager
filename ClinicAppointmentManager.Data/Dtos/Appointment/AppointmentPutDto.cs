using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentManager.Core.Enums;

namespace ClinicAppointmentManager.Core.Dtos.Appointment
{
    public class AppointmentPutDto
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public enAppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
