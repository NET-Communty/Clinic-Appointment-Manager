
namespace ClinicAppointmentManager.Core.Entities
{
    public class Specialty : BaseEntity
    {
        public int SpecialtyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
