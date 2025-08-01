using ClinicAppointmentManager.Core.Dtos;
using ClinicAppointmentManager.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponseDto>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task<DoctorResponseDto> AddAsync(DoctorPostDto doctor);
        Task UpdateAsync(Doctor doctor);
        Task DeleteAsync(int id);
    }
}
