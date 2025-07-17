using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Core.Entities
{
    public class Clinic : BaseEntity
    {
        public int ClinicId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
