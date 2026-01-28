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
    }
}
