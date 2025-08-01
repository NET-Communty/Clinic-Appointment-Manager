using ClinicAppointmentManager.Core.Dtos;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DoctorResponseDto>> GetAllAsync()
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync("Clinic,Specialty");


            var doctorDtos = doctors.Select(doctor => new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                LicenseNumber = doctor.LicenseNumber,
                SpecialtyName = doctor.Specialty.Name, // TODO: must include Specialty in the query
                ClinicName = doctor.Clinic.Name, // TODO: must include Clinic in the query
                Email = doctor.Email
            });

            return doctorDtos;
        }

        public async Task<Doctor?> GetByIdAsync(int id) {
            
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id, "Clinic,Specialty");
            if (doctor == null)
                return null;
            
            return new Doctor
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                LicenseNumber = doctor.LicenseNumber,
                Specialty = doctor.Specialty,
                Clinic = doctor.Clinic,
                Email = doctor.Email
            };


        }
        public async Task<DoctorResponseDto> AddAsync(DoctorPostDto doctor)
        {
            // Validate if the clinic and specialty exist
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(doctor.ClinicId);
            if (clinic == null)
                throw new KeyNotFoundException($"Clinic with ID {doctor.ClinicId} does not exist.");
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(doctor.SpecialtyId);
            if (specialty == null)
                throw new KeyNotFoundException($"Specialty with ID {doctor.SpecialtyId} does not exist.");

            var doctorEntity = new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                LicenseNumber = doctor.LicenseNumber,
                SpecialtyId = doctor.SpecialtyId,
                ClinicId = doctor.ClinicId
            }; 

            await _unitOfWork.Doctors.AddAsync(doctorEntity);
            await _unitOfWork.CompleteAsync();

            var response = new DoctorResponseDto
            {
                DoctorId = doctorEntity.DoctorId,
                FirstName = doctorEntity.FirstName,
                LastName = doctorEntity.LastName,
                Email = doctorEntity.Email,
                LicenseNumber = doctorEntity.LicenseNumber,
                SpecialtyName = specialty.Name,
                ClinicName = clinic.Name
            };

            return response;
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            await _unitOfWork.Doctors.UpdateAsync(doctor);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {

            await _unitOfWork.Doctors.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
