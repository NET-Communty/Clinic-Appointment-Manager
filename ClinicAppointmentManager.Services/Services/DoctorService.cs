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

        public async Task<IEnumerable<Doctor>> GetAllAsync() =>
            await _unitOfWork.Doctors.GetAllAsync();

        public async Task<Doctor?> GetByIdAsync(int id) =>
            await _unitOfWork.Doctors.GetByIdAsync(id);

        public async Task AddAsync(Doctor doctor)
        {
            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            await _unitOfWork.Doctors.UpdateAsync(doctor);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor is null)
                return;

            await _unitOfWork.Doctors.DeleteAsync(doctor);
            await _unitOfWork.CompleteAsync();
        }
    }
}
