using ClinicAppointmentManager.Core.Dtos.Clinic;
using ClinicAppointmentManager.Core.Dtos.Doctor;
using ClinicAppointmentManager.Core.Dtos.Specialty;
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
        Task<IEnumerable<DoctorResponseDto>> GetClinicDoctors(int id); // List all doctors associated with a clinic.
        Task<IEnumerable<SpecialtyResponseDto>> GetClinicSpecialties(int id); // List all specialties offered by the clinic.
    }
}
