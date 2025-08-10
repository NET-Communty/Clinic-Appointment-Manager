namespace ClinicAppointmentManager.Core.Entities
{
    public class MedicalHistory : BaseEntity
    {
        public int MedicalHistoryId { get; set; }
        public int PatientId { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public DateTime RecordDate { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
