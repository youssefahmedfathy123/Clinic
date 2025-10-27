using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController(IDoctorServices _doctorService) : ControllerBase
    {

        [HttpGet("by-category/{categoryId:guid}")]
        public async Task<IActionResult> GetDoctorsByCategory(Guid categoryId)
        {
            var doctors = await _doctorService.GetDoctorByCategory(categoryId);
            if (doctors == null || !doctors.Any())
                return NotFound("No doctors found for this category.");

            return Ok(doctors);
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return Ok("Doctor Controller is working!");
        }


        [HttpGet("by-name")]
        public async Task<IActionResult> GetDoctorsByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required.");

            var doctors = await _doctorService.GetDoctorByName(name);
            if (doctors == null || !doctors.Any())
                return NotFound("No doctors found with that name.");

            return Ok(doctors);
        }
    }
}

