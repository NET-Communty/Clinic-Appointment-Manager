using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync() => await _unitOfWork.Patients.GetAllAsync();

        public async Task<Patient?> GetByIdAsync(int id) => await _unitOfWork.Patients.GetByIdAsync(id);

        public async Task AddAsync(Patient patient)
        {
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            await _unitOfWork.Patients.UpdateAsync(patient);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (Patient is null)
                return;

            await _unitOfWork.Patients.DeleteAsync(Patient);
            await _unitOfWork.CompleteAsync();
        }
    }
}
