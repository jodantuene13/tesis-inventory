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
