using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesisInventory.Application.Interfaces;

namespace TesisInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SedesController : ControllerBase
    {
        private readonly ISedesService _sedesService;

        public SedesController(ISedesService sedesService)
        {
            _sedesService = sedesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sedes = await _sedesService.GetAllSedesAsync();
            return Ok(sedes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sede = await _sedesService.GetSedeByIdAsync(id);
            if (sede == null) return NotFound();
            return Ok(sede);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TesisInventory.Application.DTOs.Sedes.CreateSedeDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sede = await _sedesService.CreateSedeAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = sede.IdSede }, sede);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TesisInventory.Application.DTOs.Sedes.UpdateSedeDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sede = await _sedesService.UpdateSedeAsync(id, updateDto);
            if (sede == null) return NotFound();

            return Ok(sede);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _sedesService.DeleteSedeAsync(id);
                if (!deleted) return NotFound();

                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
