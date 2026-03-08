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
        public DbSet<Sede> Sede { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Inventory DbSets
        public DbSet<Rubro> Rubro { get; set; }
        public DbSet<Familia> Familia { get; set; }
        public DbSet<Atributo> Atributo { get; set; }
        public DbSet<FamiliaAtributo> FamiliaAtributo { get; set; }
        public DbSet<AtributoOpcion> AtributoOpcion { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<ProductoAtributoValor> ProductoAtributoValor { get; set; }
        public DbSet<Stock> Stock { get; set; }

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

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Sede)
                .WithMany(s => s.Usuarios)
                .HasForeignKey(u => u.IdSede);

            modelBuilder.Entity<Sede>().ToTable("Sede");
            modelBuilder.Entity<Sede>().HasKey(s => s.IdSede);
            modelBuilder.Entity<Sede>().Property(s => s.IdSede).HasColumnName("idSede");
            modelBuilder.Entity<Sede>().Property(s => s.NombreSede).HasColumnName("nombreSede");

            // Audit
            modelBuilder.Entity<AuditLog>().ToTable("AuditLog");
            modelBuilder.Entity<AuditLog>().HasKey(a => a.Id);

            // Inventory Module Configurations
            modelBuilder.Entity<Rubro>().ToTable("Rubro");
            modelBuilder.Entity<Rubro>().HasKey(r => r.IdRubro);
            modelBuilder.Entity<Rubro>().HasIndex(r => r.CodigoRubro).IsUnique();

            modelBuilder.Entity<Familia>().ToTable("Familia");
            modelBuilder.Entity<Familia>().HasKey(f => f.IdFamilia);
            modelBuilder.Entity<Familia>().HasIndex(f => new { f.IdRubro, f.CodigoFamilia }).IsUnique();
            modelBuilder.Entity<Familia>()
                .HasOne(f => f.Rubro)
                .WithMany(r => r.Familias)
                .HasForeignKey(f => f.IdRubro)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Atributo>().ToTable("Atributo");
            modelBuilder.Entity<Atributo>().HasKey(a => a.IdAtributo);
            modelBuilder.Entity<Atributo>().HasIndex(a => a.CodigoAtributo).IsUnique();

            modelBuilder.Entity<FamiliaAtributo>().ToTable("FamiliaAtributo");
            modelBuilder.Entity<FamiliaAtributo>().HasKey(fa => fa.IdFamiliaAtributo);
            modelBuilder.Entity<FamiliaAtributo>().HasIndex(fa => new { fa.IdFamilia, fa.IdAtributo }).IsUnique();
            modelBuilder.Entity<FamiliaAtributo>()
                .HasOne(fa => fa.Familia)
                .WithMany(f => f.FamiliaAtributos)
                .HasForeignKey(fa => fa.IdFamilia)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<FamiliaAtributo>()
                .HasOne(fa => fa.Atributo)
                .WithMany(a => a.FamiliaAtributos)
                .HasForeignKey(fa => fa.IdAtributo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AtributoOpcion>().ToTable("AtributoOpcion");
            modelBuilder.Entity<AtributoOpcion>().HasKey(ao => ao.IdAtributoOpcion);
            modelBuilder.Entity<AtributoOpcion>()
                .HasOne(ao => ao.Atributo)
                .WithMany(a => a.Opciones)
                .HasForeignKey(ao => ao.IdAtributo)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Producto>().HasKey(p => p.IdProducto);
            modelBuilder.Entity<Producto>().HasIndex(p => p.Sku).IsUnique();
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Familia)
                .WithMany(f => f.Productos)
                .HasForeignKey(p => p.IdFamilia)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductoAtributoValor>().ToTable("ProductoAtributoValor");
            modelBuilder.Entity<ProductoAtributoValor>().HasKey(pav => pav.IdProductoAtributoValor);
            modelBuilder.Entity<ProductoAtributoValor>().HasIndex(pav => new { pav.IdProducto, pav.IdAtributo }).IsUnique();
            modelBuilder.Entity<ProductoAtributoValor>()
                .HasOne(pav => pav.Producto)
                .WithMany(p => p.ProductoAtributoValores)
                .HasForeignKey(pav => pav.IdProducto)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductoAtributoValor>()
                .HasOne(pav => pav.Atributo)
                .WithMany()
                .HasForeignKey(pav => pav.IdAtributo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Stock>().ToTable("Stock");
            modelBuilder.Entity<Stock>().HasKey(s => s.IdStock);
            modelBuilder.Entity<Stock>().HasIndex(s => new { s.IdProducto, s.IdSede }).IsUnique();
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Producto)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s => s.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Sede)
                .WithMany()
                .HasForeignKey(s => s.IdSede)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
