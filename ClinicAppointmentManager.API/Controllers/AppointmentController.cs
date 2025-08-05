using ClinicAppointmentManager.Core.Dtos.Appointment;
using ClinicAppointmentManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentService.GetAllAsync();
            return Ok(appointments);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment is null)
                return NotFound();

            return Ok(appointment);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentPostDto dto)
        {
            var created = await _appointmentService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.AppointmentId }, created);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AppointmentPutDto dto)
        {
            await _appointmentService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpGet("CheckAvailability")]
        public async Task<IActionResult> CheckAvailability(int doctorId, DateTime appointmentDate)
        {
            var available = await _appointmentService.CheckAvailabilityAsync(doctorId, appointmentDate);
            return Ok(new { available });
        }
        [HttpPut("Reschedule")]
        public async Task<IActionResult> Reschedule([FromBody] RescheduleAppointmentDto dto)
        {
            await _appointmentService.RescheduleAsync(dto.AppointmentId, dto.NewDate);
            return NoContent();
        }
        [HttpPut("{id}/Cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _appointmentService.CancelAsync(id);
            return NoContent();
        }
        [HttpGet("{id}/Details")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment is null)
                return NotFound();

            return Ok(appointment);
        }
        [HttpGet("Upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments(int? doctorId = null, int? patientId = null)
        {
            var appointments = await _appointmentService.GetUpcomingAppointmentsAsync(doctorId, patientId);
            return Ok(appointments);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return NoContent();
        }



    }
}
