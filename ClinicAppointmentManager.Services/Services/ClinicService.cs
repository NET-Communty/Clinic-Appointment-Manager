using ClinicAppointmentManager.Core.Dtos.Clinic;
using ClinicAppointmentManager.Core.Dtos.Doctor;
using ClinicAppointmentManager.Core.Dtos.Specialty;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClinicResponseDto>> GetAllAsync(int page = 1)
        {
            var clinics = await _unitOfWork.Clinics.GetAllAsync("", page);
            return clinics.Select(c => new ClinicResponseDto
            {
                ClinicId = c.ClinicId,
                Name = c.Name,
                Address = c.Address,
                PhoneNumber = c.PhoneNumber
            });
        }

        public async Task<ClinicResponseDto?> GetByIdAsync(int id)
        {
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
            if (clinic == null) return null;
            return new ClinicResponseDto
            {
                ClinicId = clinic.ClinicId,
                Name = clinic.Name,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber
            };
        }

        public async Task<ClinicResponseDto> AddAsync(ClinicPostDto clinicDto)
        {
            var clinic = new Clinic
            {
                Name = clinicDto.Name,
                Address = clinicDto.Address,
                PhoneNumber = clinicDto.PhoneNumber
            };
            await _unitOfWork.Clinics.AddAsync(clinic);
            await _unitOfWork.CompleteAsync();
            return new ClinicResponseDto
            {
                ClinicId = clinic.ClinicId,
                Name = clinic.Name,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber
            };
        }

        public async Task UpdateAsync(int id, ClinicPutDto clinicDto)
        {
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
            if (clinic == null)
                throw new KeyNotFoundException($"Clinic with ID {id} not found.");
            clinic.Name = clinicDto.Name;
            clinic.Address = clinicDto.Address;
            clinic.PhoneNumber = clinicDto.PhoneNumber;
            await _unitOfWork.Clinics.UpdateAsync(clinic);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
            if (clinic == null)
                throw new KeyNotFoundException($"Clinic with ID {id} not found.");
            await _unitOfWork.Clinics.DeleteAsync(clinic.ClinicId);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<DoctorResponseDto>> GetClinicDoctors(int id)
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync("Clinic,Specialty");
            return doctors.Where(d => d.ClinicId == id).Select(doctor => new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                LicenseNumber = doctor.LicenseNumber,
                SpecialtyName = doctor.Specialty?.Name ?? string.Empty,
                ClinicName = doctor.Clinic?.Name ?? string.Empty
            });
        }

        public async Task<IEnumerable<SpecialtyResponseDto>> GetClinicSpecialties(int id)
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync("Clinic,Specialty");
            var specialties = doctors.Where(d => d.ClinicId == id && d.Specialty != null)
                .Select(d => d.Specialty)
                .Distinct()
                .Select(s => new SpecialtyResponseDto
                {
                    SpecialtyId = s.SpecialtyId,
                    Name = s.Name,
                    Description = s.Description
                });
            return specialties;
        }
    }
}
