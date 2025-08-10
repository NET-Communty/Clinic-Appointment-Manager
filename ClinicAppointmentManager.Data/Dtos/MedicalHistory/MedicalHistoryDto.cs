using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Core.Dtos.MedicalHistory
{
    public class MedicalHistoryDto
    {
        public int MedicalHistoryId { get; set; }
        public int PatientId { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public DateTime RecordDate { get; set; }
    }
}
