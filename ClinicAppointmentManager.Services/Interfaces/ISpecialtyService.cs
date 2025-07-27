using ClinicAppointmentManager.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface ISpecialtyService
    {
        Task<IEnumerable<Specialty>> GetAllAsync();
        Task<Specialty?> GetByIdAsync(int id);
        Task AddAsync(Specialty specialty);
        Task UpdateAsync(Specialty specialty);
        Task DeleteAsync(int id);
    }
}