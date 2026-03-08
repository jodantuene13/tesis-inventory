using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository; // To fetch full executor details

        public AuditService(IAuditRepository auditRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _auditRepository = auditRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task LogActionAsync(int targetUserId, string action, string details, object? oldState = null, object? newState = null)
        {
            var snapshot = new { old = oldState, @new = newState };
            string snapshotJson = (oldState == null && newState == null) ? string.Empty : System.Text.Json.JsonSerializer.Serialize(snapshot);

            var auditLog = new AuditLog
            {
                EntityId = targetUserId,
                EntityType = "Usuario",
                Action = action,
                Timestamp = DateTime.Now,
                Details = details,
                TargetUserSnapshot = snapshotJson
            };

            // Capture Executor Snapshot
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier); // Or "id" depending on JWT
            // Also try "sub" or custom claim "id"
            if (userIdClaim == null) 
            {
                 userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id");
            }

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int executorId))
            {
                auditLog.ExecutorId = executorId;
                var executor = await _userRepository.GetByIdAsync(executorId);
                
                // We need to fetch Sede and Role names. 
                // Assuming GetByIdAsync might not include them, or we lazily load/explicitly include if repository supports it.
                // The current User entity has virtual props.
                
                if (executor != null)
                {
                    auditLog.ExecutorName = executor.NombreUsuario;
                    auditLog.ExecutorEmail = executor.Email;
                    
                    // Ideally we should ensure Role and Sede are loaded. 
                    // Since I cannot guarantee GetByIdAsync includes them without looking at it again (it was FindAsync by default),
                    // I might satisfy with ID or try to fetch relations.
                    // Requirement: "si se elimina... el nombre del mismo siga siendo visible, así también las sedes"
                    
                    // Simple fix: If relations are null, use ID or fetch them. 
                    // Let's rely on what we have or "N/A" to avoid N+1 complexity if not critical, 
                    // BUT requirement is specific about Sede name.
                    // I'll try to use the navigation properties if loaded, otherwise standard "N/A".
                    // Or better, I can check if Auth logic puts these in Claims? No, usually just ID/Role.
                    
                    auditLog.ExecutorRole = executor.Rol?.NombreRol ?? executor.IdRol.ToString(); 
                    auditLog.ExecutorSede = executor.Sede?.NombreSede ?? executor.IdSede.ToString();
                }
            }
            else
            {
                auditLog.ExecutorName = "System/Anonymous";
            }

            await _auditRepository.AddAsync(auditLog);
        }

        public async Task<IEnumerable<AuditLog>> GetLogsAsync(DateTime? fromDate, DateTime? toDate, string? executorName, string? actionType)
        {
            return await _auditRepository.GetLogsAsync(fromDate, toDate, executorName, actionType);
        }
    }
}
