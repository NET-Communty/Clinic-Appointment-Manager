using ClinicAppointmentManager.Core.Dtos.MedicalHistory;
using ClinicAppointmentManager.Core.Dtos.Patient;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
        {
            var patients = await _patientService.GetAllAsync();
            var result = patients.Select(p => new PatientDto
            {
                PatientId = p.PatientId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Address = p.Address,
                PhoneNumber = p.PhoneNumber,
                DateOfBirth = p.DateOfBirth
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null) return NotFound();

            var dto = new PatientDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PatientCreateDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth
            };

            await _patientService.AddAsync(patient);
            return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PatientUpdateDto dto)
        {
            var existingPatient = await _patientService.GetByIdAsync(id);
            if (existingPatient == null) return NotFound();

            existingPatient.FirstName = dto.FirstName;
            existingPatient.LastName = dto.LastName;
            existingPatient.Address = dto.Address;
            existingPatient.PhoneNumber = dto.PhoneNumber;
            existingPatient.DateOfBirth = dto.DateOfBirth;

            await _patientService.UpdateAsync(existingPatient);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingPatient = await _patientService.GetByIdAsync(id);
            if (existingPatient == null) return NotFound();

            await _patientService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/appointments")]
        public async Task<ActionResult> GetPatientAppointments(int id)
        {
            var appointments = await _patientService.GetPatientAppointmentsAsync(id);
            return Ok(appointments);
        }

        [HttpGet("{id}/doctors")]
        public async Task<ActionResult> GetPatientDoctors(int id)
        {
            var doctors = await _patientService.GetPatientDoctorsAsync(id);
            return Ok(doctors);
        }

        [HttpPost("{id}/request-appointment")]
        public async Task<ActionResult> RequestAppointment(
            int id, int doctorId, DateTime appointmentDate, int durationMinutes)
        {
            await _patientService.RequestAppointmentAsync(id, doctorId, appointmentDate, durationMinutes);
            return Ok(new { message = "Appointment requested successfully" });
        }
        [HttpGet("{id}/medical-history")]
        public async Task<ActionResult> GetMedicalHistory(int id)
        {
            var history = await _patientService.GetPatientMedicalHistoryAsync(id);
            return Ok(history);
        }

        [HttpPost("{id}/medical-history")]
        public async Task<ActionResult> AddMedicalHistory(int id, [FromBody] MedicalHistoryCreateDto dto)
        {
            await _patientService.AddMedicalHistoryAsync(id, dto);
            return Ok(new { message = "Medical history added successfully" });
        }

    }
}
