using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Clinic>> GetAllClinicsAsync()
        {
            return await _unitOfWork.Clinics.GetAllAsync();
        }

        public async Task<Clinic?> GetClinicByIdAsync(int id)
        {
            return await _unitOfWork.Clinics.GetByIdAsync(id);
        }

        public async Task<Clinic> CreateClinicAsync(Clinic clinic)
        {
            await _unitOfWork.Clinics.AddAsync(clinic);
            await _unitOfWork.CompleteAsync();
            return clinic;
        }

        public async Task UpdateClinicAsync(Clinic clinic)
        {
            await _unitOfWork.Clinics.UpdateAsync(clinic);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteClinicAsync(int id)
        {
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
            if (clinic != null)
            {
                await _unitOfWork.Clinics.DeleteAsync(clinic.ClinicId);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
