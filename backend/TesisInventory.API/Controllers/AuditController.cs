using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TesisInventory.Application.Interfaces;

namespace TesisInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? executorName,
            [FromQuery] string? actionType)
        {
            var logs = await _auditService.GetLogsAsync(fromDate, toDate, executorName, actionType);
            return Ok(logs);
        }
    }
}
