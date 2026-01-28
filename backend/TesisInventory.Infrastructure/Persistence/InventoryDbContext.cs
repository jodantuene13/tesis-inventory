using Microsoft.EntityFrameworkCore;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Infrastructure.Persistence
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; } // Singular naming as per DB refactor
        public DbSet<Rol> Rol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map entities to singular table names if EF defaults to plural
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Rol>().ToTable("Rol");

            // Key configurations
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Rol>().HasKey(r => r.IdRol);

            // Column Mappings (Case sensitive / Special characters fix)
            modelBuilder.Entity<Usuario>().Property(u => u.IdUsuario).HasColumnName("idUsuario");
            modelBuilder.Entity<Usuario>().Property(u => u.NombreUsuario).HasColumnName("nombreUsuario");
            modelBuilder.Entity<Usuario>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<Usuario>().Property(u => u.GoogleId).HasColumnName("googleId");
            modelBuilder.Entity<Usuario>().Property(u => u.Password).HasColumnName("password");
            modelBuilder.Entity<Usuario>().Property(u => u.Estado).HasColumnName("estado");
            modelBuilder.Entity<Usuario>().Property(u => u.FechaRegistro).HasColumnName("fechaRegistro");
            modelBuilder.Entity<Usuario>().Property(u => u.IdRol).HasColumnName("idRol");
            modelBuilder.Entity<Usuario>().Property(u => u.IdSede).HasColumnName("idSede");

            modelBuilder.Entity<Rol>().Property(r => r.IdRol).HasColumnName("idRol");
            modelBuilder.Entity<Rol>().Property(r => r.NombreRol).HasColumnName("nombreRol");
            modelBuilder.Entity<Rol>().Property(r => r.Descripcion).HasColumnName("descripcion");

            // Relationships
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol);
        }
    }
}
