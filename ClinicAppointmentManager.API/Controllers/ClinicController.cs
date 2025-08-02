using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentManager.Services.Interfaces;
using ClinicAppointmentManager.Core.Dtos;

namespace ClinicAppointmentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;

        public ClinicController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var clinics = await _clinicService.GetAllAsync();
            if (clinics == null || !clinics.Any())
            {
                return NotFound("No clinics found.");
            }
            return Ok(clinics);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var clinic = await _clinicService.GetByIdAsync(id);
            if (clinic == null)
            {
                return NotFound($"Clinic with ID {id} not found.");
            }
            return Ok(clinic);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] ClinicPostDto clinic)
        {
            if (clinic == null)
            {
                return BadRequest("Clinic data is null.");
            }
            try
            {
                var clinicResponse = await _clinicService.AddAsync(clinic);
                return CreatedAtAction(nameof(GetById), new { id = clinicResponse.ClinicId }, clinicResponse);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest($"Error adding clinic: {ex.Message}");
            }
        }

        [HttpPut("Update/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int Id, [FromBody] ClinicPutDto clinic)
        {
            if (clinic == null)
            {
                return BadRequest("Clinic data is null.");
            }
            try
            {
                await _clinicService.UpdateAsync(Id, clinic);
                return Ok("Clinic updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest($"Error updating clinic: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _clinicService.DeleteAsync(Id);
                return Ok("Clinic deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest($"Error deleting clinic: {ex.Message}");
            }
        }
    }
}
