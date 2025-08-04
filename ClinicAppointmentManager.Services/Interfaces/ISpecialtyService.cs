using ClinicAppointmentManager.Core.Dtos.Clinic;
using ClinicAppointmentManager.Core.Dtos.Doctor;
using ClinicAppointmentManager.Core.Dtos.Specialty;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface ISpecialtyService
    {
        Task<IEnumerable<SpecialtyResponseDto>> GetAllAsync();
        Task<SpecialtyResponseDto?> GetByIdAsync(int id);
        Task<SpecialtyResponseDto> AddAsync(SpecialtyPostDto specialtyDto);
        Task UpdateAsync(int id, SpecialtyPutDto specialtyDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<DoctorResponseDto>> GetSpecialtyDoctorsAsync(int specialtyId);
        Task<IEnumerable<ClinicResponseDto>> GetSpecialtyClinicsAsync(int specialtyId);
    }
}