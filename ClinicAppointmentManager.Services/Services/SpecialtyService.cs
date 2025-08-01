using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialtyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Specialty>> GetAllAsync() => await _unitOfWork.Specialties.GetAllAsync();

        public async Task<Specialty?> GetByIdAsync(int id) => await _unitOfWork.Specialties.GetByIdAsync(id);

        public async Task AddAsync(Specialty specialty)
        {
            await _unitOfWork.Specialties.AddAsync(specialty);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Specialty specialty)
        {
            await _unitOfWork.Specialties.UpdateAsync(specialty);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {

            await _unitOfWork.Specialties.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

    }
}
