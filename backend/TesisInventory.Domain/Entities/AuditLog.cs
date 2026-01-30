using System;

namespace TesisInventory.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        
        // Target Entity
        public int EntityId { get; set; }
        public string EntityType { get; set; } = "Usuario"; // Default for now
        
        // Action
        public string Action { get; set; } = string.Empty; // CREATE, UPDATE, DELETE, STATUS_CHANGE
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Details { get; set; } = string.Empty; // Description or JSON of changes
        
        // Executor Snapshot (Persisted even if user is deleted)
        public int? ExecutorId { get; set; }
        public string ExecutorName { get; set; } = string.Empty;
        public string ExecutorEmail { get; set; } = string.Empty;
        public string ExecutorRole { get; set; } = string.Empty;
        public string ExecutorSede { get; set; } = string.Empty;

        // Target Snapshot (Optional, for detailed history)
        public string TargetUserSnapshot { get; set; } = string.Empty; // JSON of the user state
    }
}
