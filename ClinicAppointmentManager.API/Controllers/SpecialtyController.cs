using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentManager.Services.Interfaces;
using ClinicAppointmentManager.Core.Dtos.Specialty;

namespace ClinicAppointmentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            var specialties = await _specialtyService.GetAllAsync(page);
            if (specialties == null || !specialties.Any())
            {
                return NotFound("No specialties found.");
            }
            return Ok(specialties);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var specialty = await _specialtyService.GetByIdAsync(id);
            if (specialty == null)
            {
                return NotFound($"Specialty with ID {id} not found.");
            }
            return Ok(specialty);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] SpecialtyPostDto specialty)
        {
            if (specialty == null)
            {
                return BadRequest("Specialty data is null.");
            }
            try
            {
                var specialtyResponse = await _specialtyService.AddAsync(specialty);
                return CreatedAtAction(nameof(GetById), new { id = specialtyResponse.SpecialtyId }, specialtyResponse);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest($"Error adding specialty: {ex.Message}");
            }
        }

        [HttpPut("Update/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int Id, [FromBody] SpecialtyPutDto specialty)
        {
            if (specialty == null)
            {
                return BadRequest("Specialty data is null.");
            }
            try
            {
                await _specialtyService.UpdateAsync(Id, specialty);
                return Ok("Specialty updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest($"Error updating specialty: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _specialtyService.DeleteAsync(Id);
                return Ok("Specialty deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest($"Error deleting specialty: {ex.Message}");
            }
        }

        [HttpGet("{specialtyId}/Doctors")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpecialtyDoctors(int specialtyId)
        {
            var doctors = await _specialtyService.GetSpecialtyDoctorsAsync(specialtyId);
            if (doctors == null || !doctors.Any())
            {
                return NotFound($"No doctors found for specialty ID {specialtyId}.");
            }
            return Ok(doctors);
        }

        [HttpGet("{specialtyId}/Clinics")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpecialtyClinics(int specialtyId)
        {
            var clinics = await _specialtyService.GetSpecialtyClinicsAsync(specialtyId);
            if (clinics == null || !clinics.Any())
            {
                return NotFound($"No clinics found for specialty ID {specialtyId}.");
            }
            return Ok(clinics);
        }
    }
}
