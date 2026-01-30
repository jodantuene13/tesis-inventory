using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Application.Interfaces
{
    public interface IAuditService
    {
        Task LogActionAsync(int targetUserId, string action, string details, object? oldState = null, object? newState = null);
        Task<IEnumerable<AuditLog>> GetLogsAsync(System.DateTime? fromDate, System.DateTime? toDate, string? executorName, string? actionType);
    }
}
