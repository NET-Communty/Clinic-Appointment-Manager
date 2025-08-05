using ClinicAppointmentManager.Core.Dtos.Appointment;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Enums;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync("Doctor,Patient");

            return appointments.Select(a => new AppointmentResponseDto
            {
                AppointmentId = a.AppointmentId,
                AppointmentDate = a.AppointmentDate,
                DurationMinutes = a.DurationMinutes,
                Status = a.Status.ToString(),
                DoctorName = a.Doctor.FullName,
                PatientName = a.Patient.FullName
            });
        }

        public async Task<AppointmentResponseDto?> GetByIdAsync(int id)
        {
            var a = await _unitOfWork.Appointments.GetByIdAsync(id, "Doctor,Patient");

            if (a is null) return null;

            return new AppointmentResponseDto
            {
                AppointmentId = a.AppointmentId,
                AppointmentDate = a.AppointmentDate,
                DurationMinutes = a.DurationMinutes,
                Status = a.Status.ToString(),
                DoctorName = a.Doctor.FullName,
                PatientName = a.Patient.FullName
            };
        }

        public async Task<AppointmentResponseDto> AddAsync(AppointmentPostDto dto)
        {
            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                AppointmentDate = dto.AppointmentDate,
                DurationMinutes = dto.DurationMinutes,
                Status = enAppointmentStatus.Scheduled
            };

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();

            appointment = await _unitOfWork.Appointments.GetByIdAsync(appointment.AppointmentId, "Doctor,Patient");

            return new AppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                AppointmentDate = appointment.AppointmentDate,
                DurationMinutes = appointment.DurationMinutes,
                Status = appointment.Status.ToString(),
                DoctorName = appointment.Doctor.FullName,
                PatientName = appointment.Patient.FullName
            };
        }

        public async Task UpdateAsync(AppointmentPutDto dto)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
                throw new Exception("Appointment not found");

            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.DurationMinutes = dto.DurationMinutes;
            appointment.Status = dto.Status;
            appointment.Notes = dto.Notes;

            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Appointments.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
        // check if a doctor is available for an appointment on a specific date
       
        public async Task<bool> CheckAvailabilityAsync(int doctorId, DateTime appointmentDate)
        {
            var appointments = await _unitOfWork.Appointments
                .FindAsync(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate && a.Status == enAppointmentStatus.Scheduled);

            return !appointments.Any();
        }
        public async Task CancelAsync(int appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            if (appointment is null)
                throw new Exception("Appointment not found");

            appointment.Status = enAppointmentStatus.Cancelled;

            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }
        public async Task<IEnumerable<AppointmentResponseDto>> GetUpcomingAppointmentsAsync(int? doctorId, int? patientId)
        {
            var query = await _unitOfWork.Appointments.GetAllAsync("Doctor,Patient");

            var upcoming = query
                .Where(a => a.AppointmentDate > DateTime.Now &&
                    (doctorId == null || a.DoctorId == doctorId) &&
                    (patientId == null || a.PatientId == patientId))
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    DurationMinutes = a.DurationMinutes,
                    Status = a.Status.ToString(),
                    DoctorName = a.Doctor.FullName,
                    PatientName = a.Patient.FullName
                });

            return upcoming;
        }
        public async Task RescheduleAsync(int appointmentId, DateTime newDate)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            if (appointment is null)
                throw new Exception("Appointment not found");

            appointment.AppointmentDate = newDate;

            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }


    }
}
