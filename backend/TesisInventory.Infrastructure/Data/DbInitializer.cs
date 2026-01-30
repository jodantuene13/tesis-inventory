using System.Linq;
using TesisInventory.Domain.Entities;
using TesisInventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TesisInventory.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InventoryDbContext context)
        {
            context.Database.EnsureCreated();
            
            // Ensure AuditLog table exists (Fix for missing migration)
            context.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS AuditLog (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    EntityId INT NOT NULL,
                    EntityType VARCHAR(50) DEFAULT 'Usuario',
                    Action VARCHAR(50) NOT NULL,
                    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                    Details TEXT,
                    ExecutorId INT,
                    ExecutorName VARCHAR(100),
                    ExecutorEmail VARCHAR(100),
                    ExecutorRole VARCHAR(50),
                    ExecutorSede VARCHAR(50),
                    TargetUserSnapshot TEXT
                );
            ");

            // Seed Roles
            if (!context.Rol.Any())
            {
                var roles = new Rol[]
                {
                    new Rol { IdRol = 1, NombreRol = "Administrador", Descripcion = "Acceso total al sistema" },
                    new Rol { IdRol = 2, NombreRol = "Operador", Descripcion = "Acceso limitado a operaciones diarias" }
                };
                context.Rol.AddRange(roles);
                context.SaveChanges();
            }

            // Seed Sedes
            if (!context.Sede.Any())
            {
                var sedes = new Sede[]
                {
                    new Sede { IdSede = 1, NombreSede = "Sede Central" }
                };
                context.Sede.AddRange(sedes);
                context.SaveChanges();
            }

            // Seed Users
            if (!context.Usuario.Any())
            {
                var adminUser = new Usuario
                {
                    NombreUsuario = "Admin",
                    Email = "admin@ucc.edu.ar",
                    Password = "admin", // In a real app, this should be hashed!
                    Estado = true,
                    FechaRegistro = DateTime.Now,
                    IdRol = 1, // Administrador
                    IdSede = 1 // Sede Central
                };
                context.Usuario.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
