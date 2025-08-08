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

        public async Task<IEnumerable<Patient>> GetAllAsync(int page = 1) => await _unitOfWork.Patients.GetAllAsync("",page);

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
            await _unitOfWork.Patients.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
