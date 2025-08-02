using ClinicAppointmentManager.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface IClinicService
    {
        Task<IEnumerable<ClinicResponseDto>> GetAllAsync();
        Task<ClinicResponseDto?> GetByIdAsync(int id);
        Task<ClinicResponseDto> AddAsync(ClinicPostDto clinicDto);
        Task UpdateAsync(int id, ClinicPutDto clinicDto);
        Task DeleteAsync(int id);
    }
}
