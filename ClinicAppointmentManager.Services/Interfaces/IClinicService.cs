using ClinicAppointmentManager.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface IClinicService
    {
        Task<IEnumerable<Clinic>> GetAllClinicsAsync();
        Task<Clinic?> GetClinicByIdAsync(int id);
        Task<Clinic> CreateClinicAsync(Clinic clinic);
        Task UpdateClinicAsync(Clinic clinic);
        Task DeleteClinicAsync(int id);
    }
}
