using ClinicAppointmentManager.Core.Constants;
using ClinicAppointmentManager.Core.Dtos.Doctor;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Services;
using ClinicAppointmentManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int page = 1) // return Doctor Dto list
        {
            var doctors = await _doctorService.GetAllAsync(page);

            if (doctors == null || !doctors.Any())
            {
                return NotFound("No doctors found.");
            }

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id) // return Doctor Dto
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }
            return Ok(doctor);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] DoctorPostDto doctor)
        {
            // ModelState is automatically validated by [ApiController] and FluentValidation
            if (doctor == null)
            {
                return BadRequest("Doctor data is null.");
            }
            try
            {
                var doctorResponse = await _doctorService.AddAsync(doctor);

                return CreatedAtAction(nameof(GetById),
                    new { id = doctorResponse.DoctorId },
                    doctorResponse);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest($"Error adding doctor: {ex.Message}");
            }

        }

        [HttpPut("Update/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int Id, [FromBody] DoctorPutDto doctor)
        {
            if (doctor == null)
            {
                return BadRequest("Doctor data is null.");
            }
            try
            {
                await _doctorService.UpdateAsync(Id, doctor);
                return Ok("Doctor updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest($"Error updating doctor: {ex.Message}");
            }

        }

        [HttpDelete("Delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _doctorService.DeleteAsync(Id);
                return Ok("Doctor deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Error deleting doctor: {ex.Message}");
            }
        }

        [HttpGet("{id}/Schedule")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctorSchedule(int id)
        {
            try
            {
                var schedule = await _doctorService.GetDoctorSchedule(id);
                return Ok(schedule);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("BySpecialty/{specialtyId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBySpecialty(int specialtyId)
        {
            var doctors = await _doctorService.GetBySpecialtyAsync(specialtyId);
            if (doctors == null || !doctors.Any())
            {
                return NotFound($"No doctors found for speciality Id = {specialtyId}.");
            }
            return Ok(doctors);
        }

        [HttpGet("ByClinic/{clinicId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByClinic(int clinicId)
        {
            var doctors = await _doctorService.GetByClinicAsync(clinicId);
            if (doctors == null || !doctors.Any())
            {
                return NotFound($"No doctors found for clinic Id = {clinicId}.");
            }
            return Ok(doctors);
        }

        [HttpGet("{doctorId}/UpcomingAppointments")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUpcomingAppointments(int doctorId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var appointments = await _doctorService.GetUpcomingAppointmentsAsync(doctorId, fromDate, toDate);
            if (appointments == null || !appointments.Any())
                return NotFound($"No appointments found for doctor Id = {doctorId}");
            return Ok(appointments);
        }

        [HttpGet("{doctorId}/IsAvailable")]
        public async Task<IActionResult> IsAvailable(int doctorId, [FromQuery] DateTime desiredDateTime, [FromQuery] int durationMinutes)
        {
            var available = await _doctorService.IsAvailableAsync(doctorId, desiredDateTime, durationMinutes);
            return Ok(new { Available = available });
        }

        [HttpGet("{doctorId}/DailySchedule")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDailySchedule(int doctorId, [FromQuery] DateTime date)
        {
            var schedule = await _doctorService.GetDailyScheduleAsync(doctorId, date);
            if (schedule == null || !schedule.Any())
                return NotFound($"No appointments found for doctor Id = {doctorId} in this date {date}");
            return Ok(schedule);
        }

        [HttpGet("{doctorId}/WorkloadStats")]
        public async Task<IActionResult> GetWorkloadStats(int doctorId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var stats = await _doctorService.GetWorkloadStatsAsync(doctorId, fromDate, toDate);
            return Ok(stats);
        }
    }
}
