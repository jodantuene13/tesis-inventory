using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IAuditRepository
    {
        Task AddAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetLogsAsync(DateTime? fromDate, DateTime? toDate, string? executorName, string? actionType);
    }
}
