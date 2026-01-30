using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly InventoryDbContext _context;

        public AuditRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsAsync(DateTime? fromDate, DateTime? toDate, string? executorName, string? actionType)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(l => l.Timestamp >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(l => l.Timestamp <= toDate.Value);

            if (!string.IsNullOrEmpty(executorName))
                query = query.Where(l => l.ExecutorName.Contains(executorName));

            if (!string.IsNullOrEmpty(actionType))
                query = query.Where(l => l.Action == actionType);

            return await query.OrderByDescending(l => l.Timestamp).ToListAsync();
        }
    }
}
