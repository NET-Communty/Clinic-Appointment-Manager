using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Services.Interfaces;
using ClinicAppointmentManager.Core.Enums;
using ClinicAppointmentManager.Core.Dtos.Doctor;

namespace ClinicAppointmentManager.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DoctorResponseDto>> GetAllAsync(int page = 1)
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync("Clinic,Specialty", page);


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

        public async Task<DoctorResponseDto?> GetByIdAsync(int id) 
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id, "Clinic,Specialty");
            if (doctor == null)
                return null;

            return new DoctorResponseDto {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                LicenseNumber = doctor.LicenseNumber,
                SpecialtyName = doctor.Specialty?.Name ?? string.Empty,
                ClinicName = doctor.Clinic?.Name ?? string.Empty
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

        public async Task UpdateAsync(int id,DoctorPutDto doctorDto)
        {
            var existingDoctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (existingDoctor == null)
                throw new KeyNotFoundException($"Doctor with ID {id} does not exist.");
            // Validate if the clinic and specialty exist
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(doctorDto.ClinicId);
            if (clinic == null)
                throw new KeyNotFoundException($"Clinic with ID {doctorDto.ClinicId} does not exist.");
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(doctorDto.SpecialtyId);
            if (specialty == null)
                throw new KeyNotFoundException($"Specialty with ID {doctorDto.SpecialtyId} does not exist.");

            existingDoctor.FirstName = doctorDto.FirstName;
            existingDoctor.LastName = doctorDto.LastName;
            existingDoctor.Email = doctorDto.Email;
            existingDoctor.LicenseNumber = doctorDto.LicenseNumber;
            existingDoctor.SpecialtyId = doctorDto.SpecialtyId;
            existingDoctor.ClinicId = doctorDto.ClinicId;

            await _unitOfWork.Doctors.UpdateAsync(existingDoctor);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {

            await _unitOfWork.Doctors.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<DoctorScheduleDto> GetDoctorSchedule(int doctorId)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId, "Clinic,Specialty");
            if (doctor == null)
                throw new KeyNotFoundException($"Doctor with ID {doctorId} not found.");

            // fetch doctor's appointments from database
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            var doctorAppointments = appointments.Where(a => a.DoctorId == doctorId);

            var operatingHours = doctorAppointments
                .GroupBy(a => new {
                    DayOfWeek = a.AppointmentDate.DayOfWeek,
                    Start = a.AppointmentDate.TimeOfDay,
                    End = a.AppointmentDate.TimeOfDay.Add(TimeSpan.FromMinutes(a.DurationMinutes))
                })
                .Select(g => new OperatingHourDto
                {
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    OpenTime = g.Key.Start.ToString(@"hh\:mm"),
                    CloseTime = g.Key.End.ToString(@"hh\:mm")
                })
                .Distinct()
                .OrderBy(h => h.DayOfWeek)
                .ThenBy(h => h.OpenTime)
                .ToList();

            var schedule = new DoctorScheduleDto
            {
                DoctorId = doctor.DoctorId,
                DoctorName = $"{doctor.FirstName} {doctor.LastName}",
                OperatingHours = operatingHours
            };
            return schedule;
        }

        public async Task<IEnumerable<DoctorResponseDto>> GetBySpecialtyAsync(int specialtyId)
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync("Clinic,Specialty");
            var filtered = doctors.Where(d => d.SpecialtyId == specialtyId);
            return filtered.Select(doctor => new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                LicenseNumber = doctor.LicenseNumber,
                SpecialtyName = doctor.Specialty?.Name ?? string.Empty,
                ClinicName = doctor.Clinic?.Name ?? string.Empty,
                Email = doctor.Email
            });
        }

        public async Task<IEnumerable<DoctorResponseDto>> GetByClinicAsync(int clinicId)
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync("Clinic,Specialty");
            var filtered = doctors.Where(d => d.ClinicId == clinicId);
            return filtered.Select(doctor => new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                LicenseNumber = doctor.LicenseNumber,
                SpecialtyName = doctor.Specialty?.Name ?? string.Empty,
                ClinicName = doctor.Clinic?.Name ?? string.Empty,
                Email = doctor.Email
            });
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(int doctorId, DateTime? fromDate, DateTime? toDate)
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            var filtered = appointments.Where(a => a.DoctorId == doctorId &&
                (!fromDate.HasValue || a.AppointmentDate >= fromDate.Value) &&
                (!toDate.HasValue || a.AppointmentDate <= toDate.Value));

            return filtered.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                DoctorId = a.DoctorId,
                PatientId = a.PatientId,
                AppointmentDate = a.AppointmentDate,
                DurationMinutes = a.DurationMinutes,
                Status = a.Status
            });
        }

        public async Task<bool> IsAvailableAsync(int doctorId, DateTime desiredDateTime, int durationMinutes)
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            var doctorAppointments = appointments.Where(a => a.DoctorId == doctorId);
            var desiredEnd = desiredDateTime.AddMinutes(durationMinutes);
            foreach (var a in doctorAppointments)
            {
                var apptEnd = a.AppointmentDate.AddMinutes(a.DurationMinutes);
                if (desiredDateTime < apptEnd && desiredEnd > a.AppointmentDate)
                    return false;
            }
            return true;
        }

        public async Task<IEnumerable<AppointmentDto>> GetDailyScheduleAsync(int doctorId, DateTime date)
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            var filtered = appointments.Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date);
            return filtered.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                DoctorId = a.DoctorId,
                PatientId = a.PatientId,
                AppointmentDate = a.AppointmentDate,
                DurationMinutes = a.DurationMinutes,
                Status = a.Status
            });
        }

        public async Task<DoctorWorkloadStatsDto> GetWorkloadStatsAsync(int doctorId, DateTime? fromDate, DateTime? toDate)
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();

            var filtered = appointments
                .Where(a => a.DoctorId == doctorId 
                && (!fromDate.HasValue || a.AppointmentDate >= fromDate.Value)
                && (!fromDate.HasValue || a.AppointmentDate <= toDate.Value));

            int totalAppointments = filtered.Count();
            int totalMinutes = filtered.Sum(a => a.DurationMinutes);
            int cancellations = filtered.Count(a => a.Status == enAppointmentStatus.Cancelled);
            return new DoctorWorkloadStatsDto
            {
                DoctorId = doctorId,
                TotalAppointments = totalAppointments,
                TotalHoursWorked = totalMinutes / 60.0,
                Cancellations = cancellations
            };
        }
    }
}
