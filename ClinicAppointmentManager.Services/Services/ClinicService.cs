using ClinicAppointmentManager.Core.Dtos;
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

        public async Task<IEnumerable<ClinicResponseDto>> GetAllAsync()
        {
            var clinics = await _unitOfWork.Clinics.GetAllAsync();
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
    }
}
