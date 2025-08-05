using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Core.Dtos.Appointment
{
    public class RescheduleAppointmentDto
    {
        public int AppointmentId { get; set; }
        public DateTime NewDate { get; set; }
    }
}
