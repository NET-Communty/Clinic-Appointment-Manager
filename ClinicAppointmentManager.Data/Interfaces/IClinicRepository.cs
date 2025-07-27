using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicAppointmentManager.Core.Entities;

namespace ClinicAppointmentManager.Core.Interfaces
{
    public interface IClinicRepository
    {
        Task<IEnumerable<Clinic>> GetAllAsync();
        Task<Clinic?> GetByIdAsync(int id);
        Task AddAsync(Clinic clinic);
        Task UpdateAsync(Clinic clinic);
        Task DeleteAsync(int id);
    }
}
