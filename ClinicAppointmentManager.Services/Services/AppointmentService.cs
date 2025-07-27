using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync() => await _unitOfWork.Appointments.GetAllAsync();

        public async Task<Appointment?> GetByIdAsync(int id) => await _unitOfWork.Appointments.GetByIdAsync(id);

        public async Task AddAsync(Appointment appointment)
        {
            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Appointment = await _unitOfWork.Patients.GetByIdAsync(id);
            if (Appointment is null)
                return;

            await _unitOfWork.Patients.DeleteAsync(Appointment);
            await _unitOfWork.CompleteAsync();
        }
    }
}
