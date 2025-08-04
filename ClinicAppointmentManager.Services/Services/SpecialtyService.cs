using ClinicAppointmentManager.Core.Dtos.Clinic;
using ClinicAppointmentManager.Core.Dtos.Doctor;
using ClinicAppointmentManager.Core.Dtos.Specialty;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;

namespace ClinicAppointmentManager.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialtyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SpecialtyResponseDto>> GetAllAsync()
        {
            var specialties = await _unitOfWork.Specialties.GetAllAsync();
            return specialties.Select(s => new SpecialtyResponseDto
            {
                SpecialtyId = s.SpecialtyId,
                Name = s.Name,
                Description = s.Description
            });
        }

        public async Task<SpecialtyResponseDto?> GetByIdAsync(int id)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(id);
            if (specialty == null) return null;
            return new SpecialtyResponseDto
            {
                SpecialtyId = specialty.SpecialtyId,
                Name = specialty.Name,
                Description = specialty.Description
            };
        }

        public async Task<SpecialtyResponseDto> AddAsync(SpecialtyPostDto specialtyDto)
        {
            var specialty = new Specialty
            {
                Name = specialtyDto.Name,
                Description = specialtyDto.Description
            };
            await _unitOfWork.Specialties.AddAsync(specialty);
            await _unitOfWork.CompleteAsync();
            return new SpecialtyResponseDto
            {
                SpecialtyId = specialty.SpecialtyId,
                Name = specialty.Name,
                Description = specialty.Description
            };
        }

        public async Task UpdateAsync(int id, SpecialtyPutDto specialtyDto)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(id);
            if (specialty == null)
                throw new KeyNotFoundException($"Specialty with ID {id} not found.");
            specialty.Name = specialtyDto.Name;
            specialty.Description = specialtyDto.Description;
            await _unitOfWork.Specialties.UpdateAsync(specialty);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(id);
            if (specialty == null)
                throw new KeyNotFoundException($"Specialty with ID {id} not found.");
            await _unitOfWork.Specialties.DeleteAsync(specialty.SpecialtyId);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<DoctorResponseDto>> GetSpecialtyDoctorsAsync(int specialtyId)
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync("Clinic,Specialty");
            return doctors.Where(d => d.SpecialtyId == specialtyId).Select(doctor => new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                LicenseNumber = doctor.LicenseNumber,
                SpecialtyName = doctor.Specialty?.Name ?? string.Empty,
                ClinicName = doctor.Clinic?.Name ?? string.Empty,
                Email = doctor.Email
            });
        }

        public async Task<IEnumerable<ClinicResponseDto>> GetSpecialtyClinicsAsync(int specialtyId)
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync("Clinic,Specialty");
            var clinics = doctors.Where(d => d.SpecialtyId == specialtyId && d.Clinic != null)
                .Select(d => d.Clinic)
                .Distinct()
                .Select(clinic => new ClinicResponseDto
                {
                    ClinicId = clinic.ClinicId,
                    Name = clinic.Name,
                    Address = clinic.Address,
                    PhoneNumber = clinic.PhoneNumber
                });
            return clinics;
        }
    }
}
