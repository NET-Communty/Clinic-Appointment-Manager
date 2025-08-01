using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Services.Interfaces;
using ClinicAppointmentManager.Core.Dtos;
using ClinicAppointmentManager.Core.Interfaces;

namespace ClinicAppointmentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorController(IDoctorService doctorService, IUnitOfWork unitOfWork)
        {
            _doctorService = doctorService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() // return Doctor Dto list
        {
            var doctors = await _doctorService.GetAllAsync();

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
    }
}
